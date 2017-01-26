namespace Registration.Activities.ProcessPayment
{
    using System;
    using System.Threading.Tasks;
    using MassTransit.Courier;


    public class ProcessPaymentActivity :
        Activity<ProcessPaymentArguments, ProcessPaymentLog>
    {
        public async Task<ExecutionResult> Execute(ExecuteContext<ProcessPaymentArguments> context)
        {
            var authorizationCode = "ABC123";

            return context.Completed(new Log(authorizationCode, context.Arguments.Amount));
        }

        public async Task<CompensationResult> Compensate(CompensateContext<ProcessPaymentLog> context)
        {
            return context.Compensated();
        }


        class Log :
            ProcessPaymentLog
        {
            public Log(string authorizationCode, decimal amount)
            {
                AuthorizationCode = authorizationCode;
                Amount = amount;
                ChargeDate = DateTime.Today;
            }

            public DateTime ChargeDate { get; }
            public string AuthorizationCode { get; }
            public decimal Amount { get; }
        }
    }
}