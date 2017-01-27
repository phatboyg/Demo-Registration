namespace Registration.Consumers
{
    public interface ISecurePaymentInfoService
    {
        SecurePaymentInfo GetPaymentInfo(string emailAddress, string cardNumber);
    }
}