namespace Registration.Contracts
{
    public interface LicenseData
    {
        string LicenseNumber { get; }

        LicenseCategory[] Categories { get; }
    }
}