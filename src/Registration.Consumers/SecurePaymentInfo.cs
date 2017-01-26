namespace Registration.Consumers
{
    public interface SecurePaymentInfo
    {
        string CardNumber { get; }
        string VerificationCode { get; }
        string CardholderName { get; }
        int ExpirationMonth { get; }
        int ExpirationYear { get; }
    }
}