namespace Registration.Components.Activities.ProcessPayment
{
    using System;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.Extensions.Logging;


    public class ProcessPaymentActivity :
        IActivity<ProcessPaymentArguments, ProcessPaymentLog>
    {
        readonly ILogger<ProcessPaymentActivity> _logger;

        public ProcessPaymentActivity(ILogger<ProcessPaymentActivity> logger)
        {
            _logger = logger;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<ProcessPaymentArguments> context)
        {
            _logger.LogInformation("Processing Payment: {0}", context.Arguments.Amount);

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