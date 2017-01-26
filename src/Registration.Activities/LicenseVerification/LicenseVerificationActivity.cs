namespace Registration.Activities.LicenseVerification
{
    using System.Threading.Tasks;
    using MassTransit.Courier;


    public class LicenseVerificationActivity :
        ExecuteActivity<LicenseVerificiationArguments>
    {
        public Task<ExecutionResult> Execute(ExecuteContext<LicenseVerificiationArguments> context)
        {
            throw new System.NotImplementedException();
        }
    }
}