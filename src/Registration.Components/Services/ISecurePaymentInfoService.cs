namespace Registration.Components.Services
{
    using Contracts;


    public interface ISecurePaymentInfoService
    {
        SecurePaymentInfo GetPaymentInfo(string emailAddress, string cardNumber);
    }
}