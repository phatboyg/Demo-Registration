namespace Registration.Components.Activities;

public record LicenseVerificationArguments
{
    /// <summary>
    /// Racer's license number
    /// </summary>
    public string ParticipantLicenseNumber { get; init; }

    public string EventType { get; init; }
    public string ParticipantCategory { get; init; }
}