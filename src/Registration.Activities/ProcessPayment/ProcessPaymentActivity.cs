namespace Registration.Activities.ProcessPayment
{
    using System;
    using System.Threading.Tasks;
    using MassTransit.Courier;
    using MassTransit.Courier.Exceptions;
    using MassTransit.Logging;


    public class ProcessPaymentActivity :
        Activity<ProcessPaymentArguments, ProcessPaymentLog>
    {
        static readonly ILog _log = Logger.Get<ProcessPaymentActivity>();

        public async Task<ExecutionResult> Execute(ExecuteContext<ProcessPaymentArguments> context)
        {
            _log.InfoFormat("Processing Payment: {0}", context.Arguments.Amount);

            if (context.Arguments.CardNumber == "4147")
            {
                throw new RoutingSlipException("The card number is invalid");
            }

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