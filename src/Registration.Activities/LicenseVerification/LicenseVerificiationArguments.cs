namespace Registration.Activities.LicenseVerification
{
    public interface LicenseVerificiationArguments
    {
        /// <summary>
        /// Racer's license number
        /// </summary>
        string LicenseNumber { get; }

        /// <summary>
        /// Road, Cyclocross, MountainBike
        /// </summary>
        string EventType { get; }

        /// <summary>
        /// Category of racer to check
        /// </summary>
        string Category { get; }
    }
}