namespace Registration.Contracts;

using System;


public record RegistrationDetail
{
    public Guid SubmissionId { get; init; }

    public string ParticipantEmailAddress { get; init; }
    public string ParticipantLicenseNumber { get; init; }
    public string ParticipantCategory { get; init; }

    public string CardNumber { get; init; }

    public string EventId { get; init; }
    public string RaceId { get; init; }
}