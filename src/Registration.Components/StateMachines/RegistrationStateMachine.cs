namespace Registration.Components.StateMachines;

using Contracts;
using MassTransit;


public class RegistrationStateMachine :
    MassTransitStateMachine<RegistrationState>
{
    public RegistrationStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => RegistrationStatusRequested, x =>
        {
            x.ReadOnly = true;
            x.OnMissingInstance(m => m.Fault());
        });

        Initially(
            When(EventRegistrationReceived)
                .Initialize()
                .InitiateProcessing()
                .TransitionTo(Received));

        During(Received,
            When(EventRegistrationCompleted)
                .Registered()
                .TransitionTo(Registered),
            When(LicenseVerificationFailed)
                .InvalidLicense()
                .TransitionTo(Suspended),
            When(PaymentFailed)
                .PaymentFailed()
                .TransitionTo(Suspended));

        During(Suspended,
            When(EventRegistrationReceived)
                .Initialize()
                .InitiateProcessing()
                .TransitionTo(Received));

        DuringAny(
            When(RegistrationStatusRequested)
                .Respond(x => new RegistrationStatus
                {
                    SubmissionId = x.Saga.CorrelationId,
                    ParticipantEmailAddress = x.Saga.ParticipantEmailAddress,
                    ParticipantCategory = x.Saga.ParticipantCategory,
                    ParticipantLicenseNumber = x.Saga.ParticipantLicenseNumber,
                    EventId = x.Saga.EventId,
                    RaceId = x.Saga.RaceId,
                    Status = x.Saga.CurrentState
                })
        );
    }

    public State Received { get; private set; }
    public State Registered { get; private set; }
    public State Suspended { get; private set; }

    public Event<RegistrationReceived> EventRegistrationReceived { get; private set; }
    public Event<GetRegistrationStatus> RegistrationStatusRequested { get; private set; }
    public Event<RegistrationCompleted> EventRegistrationCompleted { get; private set; }
    public Event<RegistrationLicenseVerificationFailed> LicenseVerificationFailed { get; private set; }
    public Event<RegistrationPaymentFailed> PaymentFailed { get; private set; }
}


static class RegistrationStateMachineBehaviorExtensions
{
    public static EventActivityBinder<RegistrationState, RegistrationReceived> Initialize(
        this EventActivityBinder<RegistrationState, RegistrationReceived> binder)
    {
        return binder.Then(context =>
        {
            context.Saga.ParticipantEmailAddress = context.Message.ParticipantEmailAddress;
            context.Saga.ParticipantLicenseNumber = context.Message.ParticipantLicenseNumber;
            context.Saga.ParticipantCategory = context.Message.ParticipantCategory;

            context.Saga.EventId = context.Message.EventId;
            context.Saga.RaceId = context.Message.RaceId;

            LogContext.Info?.Log("Processing: {0} ({1})", context.Message.SubmissionId, context.Message.ParticipantEmailAddress);
        });
    }

    public static EventActivityBinder<RegistrationState, RegistrationReceived> InitiateProcessing(
        this EventActivityBinder<RegistrationState, RegistrationReceived> binder)
    {
        return binder.PublishAsync(context => context.Init<ProcessRegistration>(context.Message));
    }

    public static EventActivityBinder<RegistrationState, RegistrationCompleted> Registered(
        this EventActivityBinder<RegistrationState, RegistrationCompleted> binder)
    {
        return binder.Then(context =>
            LogContext.Info?.Log("Registered: {0} ({1})", context.Message.SubmissionId, context.Saga.ParticipantEmailAddress));
    }

    public static EventActivityBinder<RegistrationState, RegistrationLicenseVerificationFailed> InvalidLicense(
        this EventActivityBinder<RegistrationState, RegistrationLicenseVerificationFailed> binder)
    {
        return binder.Then(context =>
            LogContext.Info?.Log("Invalid License: {0} ({1}) - {2}", context.Message.SubmissionId, context.Saga.ParticipantLicenseNumber,
                context.Message.ExceptionInfo.Message));
    }

    public static EventActivityBinder<RegistrationState, RegistrationPaymentFailed> PaymentFailed(
        this EventActivityBinder<RegistrationState, RegistrationPaymentFailed> binder)
    {
        return binder.Then(context =>
            LogContext.Info?.Log("Payment Failed: {0} ({1}) - {2}", context.Message.SubmissionId, context.Saga.ParticipantEmailAddress,
                context.Message.ExceptionInfo.Message));
    }
}