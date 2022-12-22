namespace Registration.Components.Activities;

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
        _logger.LogInformation("Processing Payment: {Amount}", context.Arguments.Amount);

        if (context.Arguments.CardNumber == "4147")
            throw new RoutingSlipException("The card number is invalid");
        
        if(context.Arguments.CardNumber == "187187")
            throw new TransientException("The payment provider isn't responding");

        if (context.Arguments.CardNumber == "187")
        {
            if (context.GetRetryAttempt() == 0 && context.GetRedeliveryCount() == 0)
                throw new TransientException("The payment provider isn't responding");

            if (context.GetRedeliveryCount() == 0)
                throw new LongTransientException("The payment provider isn't responding after a long time");
        }

        await Task.Delay(10);

        const string authorizationCode = "ABC123";

        return context.Completed(new
        {
            ChargeDate = DateTime.UtcNow,
            authorizationCode,
            context.Arguments.Amount,
        });
    }

    public async Task<CompensationResult> Compensate(CompensateContext<ProcessPaymentLog> context)
    {
        await Task.Delay(10);

        return context.Compensated();
    }
}