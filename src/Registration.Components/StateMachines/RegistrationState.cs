namespace Registration.Components.StateMachines;

using System;
using MassTransit;


public class RegistrationState :
    SagaStateMachineInstance
{
    public string ParticipantEmailAddress { get; set; }
    public string ParticipantLicenseNumber { get; set; }
    public string ParticipantCategory { get; set; }

    public DateTime? ParticipantLicenseExpirationDate { get; set; }
    public Guid? RegistrationId { get; set; }

    public string CardNumber { get; set; }
    
    public string EventId { get; set; }
    public string RaceId { get; set; }

    public string CurrentState { get; set; }

    public string Reason { get; set; }

    public int RetryAttempt { get; set; }
    public Guid? ScheduleRetryToken { get; set; }

    public Guid CorrelationId { get; set; }
}