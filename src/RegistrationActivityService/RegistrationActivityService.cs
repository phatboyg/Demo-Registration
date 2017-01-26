namespace RegistrationActivityService
{
    using System;
    using System.Configuration;
    using MassTransit;
    using MassTransit.AzureServiceBusTransport;
    using MassTransit.Courier;
    using Registration.Activities.EventRegistration;
    using Registration.Activities.LicenseVerification;
    using Registration.Activities.ProcessPayment;
    using Topshelf;
    using Topshelf.Logging;


    public class RegistrationActivityService :
        ServiceControl
    {
        readonly LogWriter _log = HostLogger.Get<RegistrationActivityService>();

        IBusControl _busControl;

        public bool Start(HostControl hostControl)
        {
            _log.Info("Creating bus...");

            _busControl = Bus.Factory.CreateUsingAzureServiceBus(cfg =>
            {
                var host = cfg.Host(ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"], h =>
                {
                });

                ConfigureExecuteActivity<LicenseVerificationActivity, LicenseVerificiationArguments>(cfg, host);

                ConfigureActivity<EventRegistrationActivity, EventRegistrationArguments, EventRegistrationLog>(cfg, host);

                ConfigureActivity<ProcessPaymentActivity, ProcessPaymentArguments, ProcessPaymentLog>(cfg, host);
            });

            _log.Info("Starting bus...");

            _busControl.Start();

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _log.Info("Stopping bus...");

            _busControl?.Stop();

            return true;
        }

        void ConfigureActivity<TActivity, TArguments, TLog>(IServiceBusBusFactoryConfigurator cfg, IServiceBusHost host)
            where TActivity : class, Activity<TArguments, TLog>, new()
            where TArguments : class
            where TLog : class
        {
            Uri compensateAddress = null;

            cfg.ReceiveEndpoint(host, GetCompensateActivityQueueName(typeof(TActivity)), e =>
            {
                e.PrefetchCount = 16;
                e.SubscribeMessageTopics = false;
                e.CompensateActivityHost<TActivity, TLog>();

                compensateAddress = e.InputAddress;
            });

            cfg.ReceiveEndpoint(host, GetExecuteActivityQueueName(typeof(TActivity)), e =>
            {
                e.PrefetchCount = 16;
                e.SubscribeMessageTopics = false;
                e.ExecuteActivityHost<TActivity, TArguments>(compensateAddress);
            });
        }

        void ConfigureExecuteActivity<TActivity, TArguments>(IServiceBusBusFactoryConfigurator cfg, IServiceBusHost host)
            where TActivity : class, ExecuteActivity<TArguments>, new()
            where TArguments : class
        {
            cfg.ReceiveEndpoint(host, GetExecuteActivityQueueName(typeof(TActivity)), e =>
            {
                e.PrefetchCount = 16;
                e.SubscribeMessageTopics = false;
                e.ExecuteActivityHost<TActivity, TArguments>();
            });
        }

        string GetExecuteActivityQueueName(Type activityType)
        {
            return $"execute-{activityType.Name.Replace("Activity", "").ToLowerInvariant()}";
        }

        string GetCompensateActivityQueueName(Type activityType)
        {
            return $"compensate-{activityType.Name.Replace("Activity", "").ToLowerInvariant()}";
        }
    }
}