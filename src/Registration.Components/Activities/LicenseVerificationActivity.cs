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
        var arguments = context.Arguments;

        var licenseNumber = arguments.ParticipantLicenseNumber;

        _logger.LogInformation("Verifying license: {LicenseNumber} {Category}/{EventType}", licenseNumber, arguments.ParticipantCategory, arguments.EventType);

        await Task.Delay(10);

        if (licenseNumber == "8675309")
            throw new RoutingSlipException($"The license number is invalid: {licenseNumber}");

        DateTime? expirationDate = DateTime.Today + TimeSpan.FromDays(90);

        return context.CompletedWithVariables(new { ParticipantLicenseExpirationDate = expirationDate });
    }
}