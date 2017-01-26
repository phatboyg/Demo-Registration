using Automatonymous;
using Registration.Contracts;

namespace RegistrationState
{
    public class RegistrationStateMachine :
        MassTransitStateMachine<RegistrationStateInstance>
    {
        public RegistrationStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => EventRegistrationReceived, x =>
            {
                x.CorrelateById(m => m.Message.SubmissionId);
                x.SelectId(m => m.Message.SubmissionId);

                x.InsertOnInitial = true;
                x.SetSagaFactory(context =>
                {
                    var instance = new RegistrationStateInstance();

                    InitializeInstance(instance, context.Message);

                    return instance;
                });
            });

            Initially(
                When(EventRegistrationReceived)
                    .Then(Initialize)
                    .TransitionTo(Received)
            );
        }

        public State Received { get; private set; }

        public Event<RegistrationReceived> EventRegistrationReceived { get; private set; }


        private void Initialize(BehaviorContext<RegistrationStateInstance, RegistrationReceived> context)
        {
            InitializeInstance(context.Instance, context.Data);
        }

        private static void InitializeInstance(RegistrationStateInstance instance, RegistrationReceived data)
        {
            instance.ParticipantEmailAddress = data.ParticipantEmailAddress;
            instance.ParticipantLicenseNumber = data.ParticipantLicenseNumber;
            instance.ParticipantCategory = data.ParticipantCategory;

            instance.EventId = data.EventId;
            instance.RaceId = data.RaceId;
        }
    }
}