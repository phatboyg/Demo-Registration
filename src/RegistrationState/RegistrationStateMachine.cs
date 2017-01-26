namespace RegistrationState
{
    using System;
    using System.Threading.Tasks;
    using Automatonymous;
    using GreenPipes;
    using MassTransit;
    using MassTransit.Util;
    using Registration.Contracts;


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

            Event(() => EventRegistrationCompleted, x =>
            {
                x.CorrelateById(m => m.Message.SubmissionId);
            });

            Initially(
                When(EventRegistrationReceived)
                    .Then(Initialize)
                    .ThenAsync(InitiateProcessing)
                    .TransitionTo(Received));

            During(Received,
                When(EventRegistrationCompleted)
                .Then(Complete)
                .TransitionTo(Completed));
        }

        public State Received { get; private set; }
        public State Completed { get; private set; }

        public Event<RegistrationReceived> EventRegistrationReceived { get; private set; }
        public Event<RegistrationCompleted> EventRegistrationCompleted { get; private set; }

        void Initialize(BehaviorContext<RegistrationStateInstance, RegistrationReceived> context)
        {
            InitializeInstance(context.Instance, context.Data);
        }

        void Complete(BehaviorContext<RegistrationStateInstance, RegistrationCompleted> context)
        {
        }

        async Task InitiateProcessing(BehaviorContext<RegistrationStateInstance, RegistrationReceived> context)
        {
            var registration = CreateProcessRegistration(context.Data);

            Uri destinationAddress;
            if (!EndpointConvention.TryGetDestinationAddress(registration, out destinationAddress))
            {
                throw new ConfigurationException($"The endpoint convention was not configured: {TypeMetadataCache<ProcessRegistration>.ShortName}");
            }

            await context.GetPayload<ConsumeContext>().Send(destinationAddress, registration).ConfigureAwait(false);
        }

        static void InitializeInstance(RegistrationStateInstance instance, RegistrationReceived data)
        {
            instance.ParticipantEmailAddress = data.ParticipantEmailAddress;
            instance.ParticipantLicenseNumber = data.ParticipantLicenseNumber;
            instance.ParticipantCategory = data.ParticipantCategory;

            instance.EventId = data.EventId;
            instance.RaceId = data.RaceId;
        }

        static ProcessRegistration CreateProcessRegistration(RegistrationReceived message)
        {
            return new Process(message.SubmissionId, message.ParticipantEmailAddress, message.ParticipantLicenseNumber, message.ParticipantCategory,
                message.EventId, message.RaceId);
        }


        class Process :
            ProcessRegistration
        {
            public Process(Guid submissionId, string participantEmailAddress, string participantLicenseNumber, string participantCategory, string eventId,
                string raceId)
            {
                SubmissionId = submissionId;
                ParticipantEmailAddress = participantEmailAddress;
                ParticipantLicenseNumber = participantLicenseNumber;
                ParticipantCategory = participantCategory;
                EventId = eventId;
                RaceId = raceId;

                Timestamp = DateTime.UtcNow;
            }

            public Guid SubmissionId { get; }
            public DateTime Timestamp { get; }
            public string ParticipantEmailAddress { get; }
            public string ParticipantLicenseNumber { get; }
            public string ParticipantCategory { get; }
            public string EventId { get; }
            public string RaceId { get; }
        }
    }
}