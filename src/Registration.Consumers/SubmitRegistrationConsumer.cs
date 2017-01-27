namespace Registration.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Contracts;
    using MassTransit;
    using MassTransit.Logging;


    public class SubmitRegistrationConsumer :
        IConsumer<SubmitRegistration>
    {
        readonly ILog _log = Logger.Get<SubmitRegistrationConsumer>();

        public async Task Consume(ConsumeContext<SubmitRegistration> context)
        {
            _log.InfoFormat("Registration received: {0} ({1})", context.Message.SubmissionId, context.Message.ParticipantEmailAddress);

            ValidateRegistration(context.Message);

            var received = CreateReceivedEvent(context.Message);

            await context.Publish(received).ConfigureAwait(false);

            _log.InfoFormat("Registration accepted: {0} ({1})", context.Message.SubmissionId, context.Message.ParticipantEmailAddress);
        }

        static RegistrationReceived CreateReceivedEvent(SubmitRegistration message)
        {
            return new Received(message.SubmissionId, message.ParticipantEmailAddress, message.ParticipantLicenseNumber,
                message.ParticipantCategory, message.EventId, message.RaceId, message.CardNumber);
        }

        void ValidateRegistration(SubmitRegistration message)
        {
            if (string.IsNullOrWhiteSpace(message.EventId))
                throw new ArgumentNullException(nameof(message.EventId));
            if (string.IsNullOrWhiteSpace(message.RaceId))
                throw new ArgumentNullException(nameof(message.RaceId));

            if (string.IsNullOrWhiteSpace(message.ParticipantEmailAddress))
                throw new ArgumentNullException(nameof(message.ParticipantEmailAddress));
            if (string.IsNullOrWhiteSpace(message.ParticipantCategory))
                throw new ArgumentNullException(nameof(message.ParticipantCategory));
        }


        class Received :
            RegistrationReceived
        {
            public Received(Guid submissionId, string participantEmailAddress, string participantLicenseNumber, string participantCategory, string eventId,
                string raceId, string cardNumber)
            {
                SubmissionId = submissionId;
                ParticipantEmailAddress = participantEmailAddress;
                ParticipantLicenseNumber = participantLicenseNumber;
                ParticipantCategory = participantCategory;
                EventId = eventId;
                RaceId = raceId;
                CardNumber = cardNumber;

                Timestamp = DateTime.UtcNow;
            }

            public Guid SubmissionId { get; }
            public DateTime Timestamp { get; }
            public string ParticipantEmailAddress { get; }
            public string ParticipantLicenseNumber { get; }
            public string ParticipantCategory { get; }
            public string EventId { get; }
            public string RaceId { get; }
            public string CardNumber { get; }
        }
    }
}