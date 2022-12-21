namespace Registration.Components.Activities;

using System;


public record EventRegistrationLog
{
    public Guid RegistrationId { get; init; }
    public string ParticipantEmailAddress { get; init; }
}