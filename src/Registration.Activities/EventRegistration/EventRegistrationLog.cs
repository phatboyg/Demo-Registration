namespace Registration.Activities.EventRegistration
{
    using System;


    public interface EventRegistrationLog
    {
        Guid RegistrationId { get; }
        string ParticipantEmailAddress { get; }
    }
}