namespace Registration.Components.Activities;

using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;


public class LicenseVerificationActivity :
    IExecuteActivity<LicenseVerificationArguments>
{
    readonly ILogger<LicenseVerificationActivity> _logger;

    public LicenseVerificationActivity(ILogger<LicenseVerificationActivity> logger)
    {
        _logger = logger;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<LicenseVerificationArguments> context)
    {
        _logger.LogInformation("Verifying license: {LicenseNumber}", context.Arguments.ParticipantLicenseNumber);

        await Task.Delay(100);

        if (context.Arguments.ParticipantLicenseNumber == "8675309")
            throw new RoutingSlipException($"The license number is invalid: {context.Arguments.ParticipantLicenseNumber}");

        DateTime? expirationDate = DateTime.Today + TimeSpan.FromDays(90);

        return context.CompletedWithVariables(new { ParticipantLicenseExpirationDate = expirationDate });
    }
}