namespace Registration.Activities.EventRegistration
{
    using System.Threading.Tasks;
    using MassTransit.Courier;


    public class EventRegistrationActivity :
        Activity<EventRegistrationArguments, EventRegistrationLog>
    {
        public Task<ExecutionResult> Execute(ExecuteContext<EventRegistrationArguments> context)
        {
            throw new System.NotImplementedException();
        }

        public Task<CompensationResult> Compensate(CompensateContext<EventRegistrationLog> context)
        {
            throw new System.NotImplementedException();
        }
    }
}