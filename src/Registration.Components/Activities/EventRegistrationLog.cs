namespace Registration.Components.Activities;

using System;


public interface EventRegistrationLog
{
    Guid RegistrationId { get; }
    string ParticipantEmailAddress { get; }
}