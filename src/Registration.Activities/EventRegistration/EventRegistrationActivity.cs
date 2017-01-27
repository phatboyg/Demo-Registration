namespace Registration.Activities.EventRegistration
{
    using System;
    using System.Threading.Tasks;
    using MassTransit;
    using MassTransit.Courier;
    using MassTransit.Logging;


    public class EventRegistrationActivity :
        Activity<EventRegistrationArguments, EventRegistrationLog>
    {
        static readonly ILog _log = Logger.Get<EventRegistrationActivity>();

        public async Task<ExecutionResult> Execute(ExecuteContext<EventRegistrationArguments> context)
        {
            _log.InfoFormat("Registering for event: {0} ({1})", context.Arguments.EventId, context.Arguments.ParticipantEmailAddress);

            var registrationTotal = 25.00m;

            var registrationId = NewId.NextGuid();

            _log.InfoFormat("Registered for event: {0} ({1})", registrationId, context.Arguments.ParticipantEmailAddress);

            return context.CompletedWithVariables(new Log(registrationId, context.Arguments.ParticipantEmailAddress), new
            {
                Amount = registrationTotal
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<EventRegistrationLog> context)
        {
            _log.InfoFormat("Removing registration for event: {0} ({1})", context.Log.RegistrationId, context.Log.ParticipantEmailAddress);

            return context.Compensated();
        }


        class Log :
            EventRegistrationLog
        {
            public Log(Guid registrationId, string participantEmailAddress)
            {
                RegistrationId = registrationId;
                ParticipantEmailAddress = participantEmailAddress;
            }

            public Guid RegistrationId { get; }

            public string ParticipantEmailAddress { get; }
        }
    }
}