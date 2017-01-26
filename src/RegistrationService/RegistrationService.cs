namespace RegistrationService
{
    using System.Configuration;
    using MassTransit;
    using MassTransit.AzureServiceBusTransport;
    using Registration.Consumers;
    using Topshelf;
    using Topshelf.Logging;


    public class RegistrationService :
        ServiceControl
    {
        readonly LogWriter _log = HostLogger.Get<RegistrationService>();

        IBusControl _busControl;

        public bool Start(HostControl hostControl)
        {
            _log.Info("Creating bus...");

            _busControl = Bus.Factory.CreateUsingAzureServiceBus(cfg =>
            {
                var host = cfg.Host(ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"], h =>
                {
                });

                cfg.ReceiveEndpoint(host, ConfigurationManager.AppSettings["ProcessRegistrationQueueName"], e =>
                {
                    e.PrefetchCount = 16;

                    e.Consumer<SubmitRegistrationConsumer>();
                });

                cfg.ReceiveEndpoint(host, ConfigurationManager.AppSettings["SubmitRegistrationQueueName"], e =>
                {
                    e.PrefetchCount = 16;

                    e.Consumer<SubmitRegistrationConsumer>();
                });
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
    }
}

