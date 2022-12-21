namespace Registration.Components.Activities;

public record LicenseVerificationArguments
{
    /// <summary>
    /// Racer's license number
    /// </summary>
    public string ParticipantLicenseNumber { get; init; }
}