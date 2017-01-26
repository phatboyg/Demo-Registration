namespace Registration.Activities.EventRegistration
{
    using System.Threading.Tasks;
    using MassTransit.Courier;


    public class EventRegistrationActivity :
        Activity<EventRegistrationArguments, EventRegistrationLog>
    {
        public async Task<ExecutionResult> Execute(ExecuteContext<EventRegistrationArguments> context)
        {
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