namespace Registration.Components.Services;

using Contracts;


public class SecurePaymentInfoService :
    ISecurePaymentInfoService
{
    public SecurePaymentInfo GetPaymentInfo(string emailAddress, string cardNumber)
    {
        return new SecurePaymentInfo
        {
            CardNumber = cardNumber,
            VerificationCode = "123",
            CardholderName = "FRANK UNDERHILL",
            ExpirationMonth = 12,
            ExpirationYear = 2023,
        };
    }
}