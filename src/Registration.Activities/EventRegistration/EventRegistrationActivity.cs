namespace Registration.Activities.EventRegistration
{
    using System.Threading.Tasks;
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

            return context.CompletedWithVariables(new Log(), new
            {
                Amount = registrationTotal
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<EventRegistrationLog> context)
        {
            // remove registration from database

            return context.Compensated();
        }


        class Log :
            EventRegistrationLog
        {
        }
    }
}