namespace Registration.Activities.LicenseVerification
{
    using System;
    using System.Threading.Tasks;
    using MassTransit.Courier;
    using MassTransit.Courier.Exceptions;
    using MassTransit.Logging;


    public class LicenseVerificationActivity :
        ExecuteActivity<LicenseVerificiationArguments>
    {
        static readonly ILog _log = Logger.Get<LicenseVerificationActivity>();

        public async Task<ExecutionResult> Execute(ExecuteContext<LicenseVerificiationArguments> context)
        {
            _log.InfoFormat("Verifying license: {0}", context.Arguments.LicenseNumber);

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