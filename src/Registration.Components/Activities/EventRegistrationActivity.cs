namespace Registration.Components.Activities;

using System;
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
        var arguments = context.Arguments;

        _logger.LogInformation("Registering for event: {EventId} ({Email})", arguments.EventId, arguments.ParticipantEmailAddress);

        decimal registrationTotal = 25.00m;

        if (!string.IsNullOrWhiteSpace(arguments.ParticipantLicenseNumber))
        {
            _logger.LogInformation("Participant Detail: {LicenseNumber} ({LicenseExpiration}) {Category}",
                arguments.ParticipantLicenseNumber, arguments.ParticipantLicenseExpirationDate, arguments.ParticipantCategory);

            registrationTotal = 15.0m;
        }

        await Task.Delay(10);

        Guid? registrationId = NewId.NextGuid();

        _logger.LogInformation("Registered for event: {RegistrationId} ({Email})", registrationId, arguments.ParticipantEmailAddress);

        return context.CompletedWithVariables(new
        {
            registrationId,
            arguments.ParticipantEmailAddress
        }, new
        {
            registrationId,
            Amount = registrationTotal
        });
    }

    public async Task<CompensationResult> Compensate(CompensateContext<EventRegistrationLog> context)
    {
        _logger.LogInformation("Removing registration for event: {RegistrationId} ({Email})", context.Log.RegistrationId, context.Log.ParticipantEmailAddress);

        await Task.Delay(10);

        return context.Compensated();
    }
}