namespace StateService
{
    using System.Configuration;
    using Automatonymous;
    using GreenPipes;
    using MassTransit;
    using MassTransit.AzureServiceBusTransport;
    using MassTransit.Saga;
    using Registration.Contracts;
    using RegistrationState;
    using Topshelf;
    using Topshelf.Logging;


    public class StateService :
        ServiceControl
    {
        readonly LogWriter _log = HostLogger.Get<StateService>();

        IBusControl _busControl;

        public bool Start(HostControl hostControl)
        {
            _log.Info("Creating bus...");

            _busControl = Bus.Factory.CreateUsingAzureServiceBus(cfg =>
            {
                var host = cfg.Host(ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"], h =>
                {
                });

                cfg.ReceiveEndpoint(host, ConfigurationManager.AppSettings["RegistrationStateQueueName"], e =>
                {
                    e.PrefetchCount = 16;

                    var paritioner = cfg.CreatePartitioner(8);

                    var machine = new RegistrationStateMachine();
                    var repository = new InMemorySagaRepository<RegistrationStateInstance>();

                    e.StateMachineSaga(machine, repository, x =>
                    {
                        x.Message<RegistrationReceived>(m => m.UsePartitioner(paritioner, p => p.Message.SubmissionId));
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
    }
}