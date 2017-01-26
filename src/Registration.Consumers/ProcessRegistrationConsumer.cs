namespace Registration.Consumers
{
    using System.Threading.Tasks;
    using Common;
    using Contracts;
    using MassTransit;
    using MassTransit.Courier;
    using MassTransit.Courier.Contracts;


    public class ProcessRegistrationConsumer :
        IConsumer<ProcessRegistration>
    {
        ISecurePaymentInfoService _paymentInfoService;

        public ProcessRegistrationConsumer()
        {
            _paymentInfoService = new SecurePaymentInfoService();
        }

        public async Task Consume(ConsumeContext<ProcessRegistration> context)
        {
            var routingSlip = CreateRoutingSlip(context);

            await context.Execute(routingSlip).ConfigureAwait(false);
        }

        RoutingSlip CreateRoutingSlip(ConsumeContext<ProcessRegistration> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());

            if (!string.IsNullOrWhiteSpace(context.Message.ParticipantLicenseNumber))
            {
                builder.AddActivity("LicenseVerificiation", context.GetDestinationAddress("execute-licenseverification"), new
                {
                    LicenceNumber = context.Message.ParticipantLicenseNumber,
                    EventType = "Road",
                    Category = context.Message.ParticipantCategory
                });
            }

            builder.AddActivity("EventRegistration", context.GetDestinationAddress("execute-eventregistration"), new
            {
                context.Message.ParticipantEmailAddress,
                context.Message.ParticipantLicenseNumber,
                context.Message.ParticipantCategory,
                context.Message.EventId,
                context.Message.RaceId
            });

            var paymentInfo = _paymentInfoService.GetPaymentInfo(context.Message.ParticipantEmailAddress);

            builder.AddActivity("ProcessPayment", context.GetDestinationAddress("execute-processpayment"), paymentInfo);


            builder.AddSubscription(context.SourceAddress, RoutingSlipEvents.Completed, RoutingSlipEventContents.All, async x =>
            {
                await x.Send<RegistrationCompleted>(new
                {
                    context.Message.SubmissionId
                }).ConfigureAwait(false);
            });

            return builder.Build();
        }
    }
}