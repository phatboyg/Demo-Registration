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