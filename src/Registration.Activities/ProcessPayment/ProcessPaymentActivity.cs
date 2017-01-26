namespace Registration.Activities.ProcessPayment
{
    using System.Threading.Tasks;
    using MassTransit.Courier;


    public class ProcessPaymentActivity :
        Activity<ProcessPaymentArguments, ProcessPaymentLog>
    {
        public Task<ExecutionResult> Execute(ExecuteContext<ProcessPaymentArguments> context)
        {
            throw new System.NotImplementedException();
        }

        public Task<CompensationResult> Compensate(CompensateContext<ProcessPaymentLog> context)
        {
            throw new System.NotImplementedException();
        }
    }
}