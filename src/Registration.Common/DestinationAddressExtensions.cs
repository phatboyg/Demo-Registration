namespace Registration.Common
{
    using System;
    using MassTransit;


    public static class DestinationAddressExtensions
    {
        public static Uri GetDestinationAddress(this ConsumeContext context, string queueName)
        {
            var builder = new UriBuilder
            {
                Scheme = context.SourceAddress.Scheme,
                Host = context.SourceAddress.Host,
                Path = queueName
            };

            return builder.Uri;
        }

        public static Uri GetDestinationAddress(this Uri hostAddress, string queueName)
        {
            var builder = new UriBuilder
            {
                Scheme = hostAddress.Scheme,
                Host = hostAddress.Host,
                Path = queueName
            };

            return builder.Uri;
        }
    }
}