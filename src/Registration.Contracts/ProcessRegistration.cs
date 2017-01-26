namespace Registration.Contracts
{
    using System;


    public interface ProcessRegistration
    {
        Guid SubmissionId { get; }
        DateTime Timestamp { get; }

        string ParticipantEmailAddress { get; }
        string ParticipantLicenseNumber { get; }
        string ParticipantCategory { get; }

        string EventId { get; }
        string RaceId { get; }
    }
}