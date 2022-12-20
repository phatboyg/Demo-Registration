namespace Registration.Contracts;

using System;


public record GetRegistrationStatus
{
    public Guid SubmissionId { get; init; }
}