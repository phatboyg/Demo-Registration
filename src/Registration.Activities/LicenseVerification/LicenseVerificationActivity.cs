namespace Registration.Activities.LicenseVerification
{
    using System;
    using System.Threading.Tasks;
    using MassTransit.Courier;


    public class LicenseVerificationActivity :
        ExecuteActivity<LicenseVerificiationArguments>
    {
        public async Task<ExecutionResult> Execute(ExecuteContext<LicenseVerificiationArguments> context)
        {
            // verify license with remote service

            var expirationDate = DateTime.Today + TimeSpan.FromDays(90);

            return context.CompletedWithVariables(new
            {
                LicenseExpirationDate = expirationDate
            });
        }
    }
}