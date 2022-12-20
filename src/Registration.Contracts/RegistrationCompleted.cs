namespace Registration.Contracts;

using System;


public record RegistrationCompleted
{
    public Guid SubmissionId { get; init; }
    public Guid TrackingNumber { get; init; }
}