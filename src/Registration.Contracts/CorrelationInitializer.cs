namespace Registration.Contracts;

using System.Runtime.CompilerServices;
using MassTransit;


public static class CorrelationInitializer
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void Initialize()
    {
        MessageCorrelation.UseCorrelationId<GetRegistrationStatus>(x => x.SubmissionId);
        MessageCorrelation.UseCorrelationId<ProcessRegistration>(x => x.SubmissionId);
        MessageCorrelation.UseCorrelationId<RegistrationStatus>(x => x.SubmissionId);
        MessageCorrelation.UseCorrelationId<RegistrationCompleted>(x => x.SubmissionId);
        MessageCorrelation.UseCorrelationId<RegistrationLicenseVerificationFailed>(x => x.SubmissionId);
        MessageCorrelation.UseCorrelationId<RegistrationPaymentFailed>(x => x.SubmissionId);
        MessageCorrelation.UseCorrelationId<RegistrationReceived>(x => x.SubmissionId);
        MessageCorrelation.UseCorrelationId<SubmitRegistration>(x => x.SubmissionId);
    }
}