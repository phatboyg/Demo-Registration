namespace Registration.Contracts;

public record SecurePaymentInfo
{
    public string CardNumber { get; init; }
    public string VerificationCode { get; init; }
    public string CardholderName { get; init; }
    public int ExpirationMonth { get; init; }
    public int ExpirationYear { get; init; }
}