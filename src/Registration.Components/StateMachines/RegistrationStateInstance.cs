namespace Registration.Components.StateMachines
{
    using System;
    using MassTransit;


    public class RegistrationStateInstance :
        SagaStateMachineInstance
    {
        public string ParticipantEmailAddress { get; set; }
        public string ParticipantLicenseNumber { get; set; }
        public string ParticipantCategory { get; set; }

        public string EventId { get; set; }
        public string RaceId { get; set; }

        public string CurrentState { get; set; }

        public Guid CorrelationId { get; set; }
    }
}