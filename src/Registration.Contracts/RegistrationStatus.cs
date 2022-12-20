namespace Registration.Contracts;

public record RegistrationStatus :
    RegistrationDetail
{
    public string Status { get; init; }
}