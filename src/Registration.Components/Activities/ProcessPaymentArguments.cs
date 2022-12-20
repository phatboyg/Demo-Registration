namespace Registration.Components.Activities;

public interface ProcessPaymentArguments
{
    string CardNumber { get; }
    string VerificationCode { get; }
    string CardholderName { get; }
    int ExpirationMonth { get; }
    int ExpirationYear { get; }
    decimal Amount { get; }
}