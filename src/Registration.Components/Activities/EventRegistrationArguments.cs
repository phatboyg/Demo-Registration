namespace Registration.Components.Activities;

public interface EventRegistrationArguments
{
    string ParticipantEmailAddress { get; }
    string ParticipantLicenseNumber { get; }
    string ParticipantCategory { get; }

    string EventId { get; }
    string RaceId { get; }


}