namespace Registration.Contracts
{
    public interface LicenseCategory
    {       
        LicenseCategoryType CategoryType { get; }

        string Category { get; }
    }
}