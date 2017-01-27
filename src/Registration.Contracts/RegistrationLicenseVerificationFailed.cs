namespace Registration.Contracts
{
    using System;
    using MassTransit;


    public interface RegistrationLicenseVerificationFailed
    {
        Guid SubmissionId { get; }
        DateTime Timestamp { get; }

        ExceptionInfo ExceptionInfo { get; }
    }
}