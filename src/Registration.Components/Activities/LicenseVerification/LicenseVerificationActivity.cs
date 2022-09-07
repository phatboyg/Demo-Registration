namespace Registration.Components.Activities.LicenseVerification
{
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
            _logger.LogInformation("Verifying license: {0}", context.Arguments.LicenseNumber);

            // verify license with remote service

            if (context.Arguments.LicenseNumber == "8675309")
            {
                throw new RoutingSlipException($"The license number is invalid: {context.Arguments.LicenseNumber}");
            }

            var expirationDate = DateTime.Today + TimeSpan.FromDays(90);

            return context.CompletedWithVariables(new
            {
                LicenseExpirationDate = expirationDate
            });
        }
    }
}