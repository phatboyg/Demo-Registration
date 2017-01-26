namespace Registration.Contracts
{
    using System;


    public interface RegistrationCompleted
    {
        Guid SubmissionId { get; }
        DateTime Timestamp { get; }
        Guid TrackingNumber { get; }
    }
}