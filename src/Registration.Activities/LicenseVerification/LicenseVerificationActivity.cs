namespace Registration.Activities.LicenseVerification
{
    using System;
    using System.Threading.Tasks;
    using MassTransit.Courier;
    using MassTransit.Logging;


    public class LicenseVerificationActivity :
        ExecuteActivity<LicenseVerificiationArguments>
    {
        static readonly ILog _log = Logger.Get<LicenseVerificationActivity>();

        public async Task<ExecutionResult> Execute(ExecuteContext<LicenseVerificiationArguments> context)
        {
            _log.InfoFormat("Verifying license: {0}", context.Arguments.LicenseNumber);

            // verify license with remote service

            var expirationDate = DateTime.Today + TimeSpan.FromDays(90);

            return context.CompletedWithVariables(new
            {
                LicenseExpirationDate = expirationDate
            });
        }
    }
}