namespace Registration.Components.Activities;

public record ProcessPaymentArguments
{
    public string CardNumber { get; init; }
    public string VerificationCode { get; init; }
    public string CardholderName { get; init; }
    public int ExpirationMonth { get; init; }
    public int ExpirationYear { get; init; }

    public decimal Amount { get; init; }
}