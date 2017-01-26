namespace Registration.Consumers
{
    public class SecurePaymentInfoService : 
        ISecurePaymentInfoService
    {
        public SecurePaymentInfo GetPaymentInfo(string emailAddress)
        {
            return new Info("5317012345678901", "123", "FRANK UNDERHILL", 12, 2019);
        }


        class Info :
            SecurePaymentInfo
        {
            public Info(string cardNumber, string verificationCode, string cardholderName, int expirationMonth, int expirationYear)
            {
                CardNumber = cardNumber;
                VerificationCode = verificationCode;
                CardholderName = cardholderName;
                ExpirationMonth = expirationMonth;
                ExpirationYear = expirationYear;
            }

            public string CardNumber { get; }
            public string VerificationCode { get; }
            public string CardholderName { get; }
            public int ExpirationMonth { get; }
            public int ExpirationYear { get; }
        }
    }
}