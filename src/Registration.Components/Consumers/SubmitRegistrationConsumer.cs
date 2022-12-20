namespace Registration.Components.Consumers;

using System;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;


public class SubmitRegistrationConsumer :
    IConsumer<SubmitRegistration>
{
    readonly ILogger<SubmitRegistrationConsumer> _logger;

    public SubmitRegistrationConsumer(ILogger<SubmitRegistrationConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SubmitRegistration> context)
    {
        _logger.LogInformation("Registration received: {SubmissionId} ({Email})", context.Message.SubmissionId, context.Message.ParticipantEmailAddress);

        ValidateRegistration(context.Message);

        await context.Publish<RegistrationReceived>(context.Message);

        _logger.LogInformation("Registration accepted: {SubmissionId} ({Email})", context.Message.SubmissionId, context.Message.ParticipantEmailAddress);
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
}