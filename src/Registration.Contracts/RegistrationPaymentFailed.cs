namespace Registration.Contracts
{
    using System;
    using MassTransit;


    public interface RegistrationPaymentFailed
    {
        Guid SubmissionId { get; }
        DateTime Timestamp { get; }

        ExceptionInfo ExceptionInfo { get; }
    }
}