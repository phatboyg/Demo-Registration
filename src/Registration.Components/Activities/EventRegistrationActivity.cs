namespace Registration.Components.Activities;

using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;


public class EventRegistrationActivity :
    IActivity<EventRegistrationArguments, EventRegistrationLog>
{
    readonly ILogger<EventRegistrationActivity> _logger;

    public EventRegistrationActivity(ILogger<EventRegistrationActivity> logger)
    {
        _logger = logger;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<EventRegistrationArguments> context)
    {
        _logger.LogInformation("Registering for event: {0} ({1})", context.Arguments.EventId, context.Arguments.ParticipantEmailAddress);

        const decimal registrationTotal = 25.00m;

        await Task.Delay(100);

        var registrationId = NewId.NextGuid();

        _logger.LogInformation("Registered for event: {RegistrationId} ({Email})", registrationId, context.Arguments.ParticipantEmailAddress);

        return context.CompletedWithVariables(new
        {
            registrationId,
            context.Arguments.ParticipantEmailAddress
        }, new { Amount = registrationTotal });
    }

    public async Task<CompensationResult> Compensate(CompensateContext<EventRegistrationLog> context)
    {
        _logger.LogInformation("Removing registration for event: {RegistrationId} ({Email})", context.Log.RegistrationId, context.Log.ParticipantEmailAddress);

        await Task.Delay(100);

        return context.Compensated();
    }
}