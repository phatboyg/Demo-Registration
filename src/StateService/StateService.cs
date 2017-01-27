namespace StateService
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Automatonymous;
    using GreenPipes;
    using MassTransit;
    using MassTransit.AzureServiceBusTransport;
    using MassTransit.EntityFrameworkIntegration;
    using MassTransit.EntityFrameworkIntegration.Saga;
    using MassTransit.Saga;
    using Registration.Common;
    using Registration.Contracts;
    using RegistrationState;
    using Topshelf;
    using Topshelf.Logging;


    public class StateService :
        ServiceControl
    {
        readonly LogWriter _log = HostLogger.Get<StateService>();

        IBusControl _busControl;
        ISagaRepository<RegistrationStateInstance> _sagaRepository;

        public bool Start(HostControl hostControl)
        {
            _log.Info("Creating bus...");

            var connectionString = ConfigurationManager.AppSettings["DatabaseConnectionString"];

            SagaDbContextFactory sagaDbContextFactory = () => new SagaDbContext<RegistrationStateInstance, RegistrationStateInstanceMap>(connectionString);

            _sagaRepository = new EntityFrameworkSagaRepository<RegistrationStateInstance>(sagaDbContextFactory);

            _busControl = Bus.Factory.CreateUsingAzureServiceBus(cfg =>
            {
                var host = cfg.Host(ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"], h =>
                {
                });

                EndpointConvention.Map<ProcessRegistration>(
                    host.Settings.ServiceUri.GetDestinationAddress(ConfigurationManager.AppSettings["ProcessRegistrationQueueName"]));

                cfg.ReceiveEndpoint(host, ConfigurationManager.AppSettings["RegistrationStateQueueName"], e =>
                {
                    e.PrefetchCount = 16;

                    var paritioner = cfg.CreatePartitioner(8);

                    var machine = new RegistrationStateMachine();

                    e.StateMachineSaga(machine, _sagaRepository, x =>
                    {
                        x.Message<RegistrationReceived>(m => m.UsePartitioner(paritioner, p => p.Message.SubmissionId));
                        x.Message<RegistrationCompleted>(m => m.UsePartitioner(paritioner, p => p.Message.SubmissionId));
                    });
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

        public Uri GetDestinationAddress(IServiceBusHost host, string queueName)
        {
            IEnumerable<string> segments = new[] {host.Settings.ServiceUri.AbsolutePath.Trim('/'), queueName.Trim('/')}
                .Where(x => x.Length > 0);

            var builder = new UriBuilder
            {
                Scheme = host.Settings.ServiceUri.Scheme,
                Host = host.Settings.ServiceUri.Host,
                Path = string.Join("/", segments)
            };

            return builder.Uri;
        }
    }
}