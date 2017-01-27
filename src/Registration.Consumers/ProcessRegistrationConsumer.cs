namespace Registration.Consumers
{
    using System.Threading.Tasks;
    using Common;
    using Contracts;
    using MassTransit;
    using MassTransit.Courier;
    using MassTransit.Courier.Contracts;
    using MassTransit.Logging;


    public class ProcessRegistrationConsumer :
        IConsumer<ProcessRegistration>
    {
        static readonly ILog _log = Logger.Get<ProcessRegistrationConsumer>();

        readonly ISecurePaymentInfoService _paymentInfoService;

        public ProcessRegistrationConsumer()
        {
            _paymentInfoService = new SecurePaymentInfoService();
        }

        public async Task Consume(ConsumeContext<ProcessRegistration> context)
        {
            _log.InfoFormat("Processing registration: {0} ({1})", context.Message.SubmissionId, context.Message.ParticipantEmailAddress);

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
                    LicenseNumber = context.Message.ParticipantLicenseNumber,
                    EventType = "Road",
                    Category = context.Message.ParticipantCategory
                });

                builder.AddSubscription(context.SourceAddress, RoutingSlipEvents.ActivityFaulted, RoutingSlipEventContents.None, "LicenseVerificiation",
                    x => x.Send<RegistrationLicenseVerificationFailed>(new
                    {
                        context.Message.SubmissionId
                    }));
            }

            builder.AddActivity("EventRegistration", context.GetDestinationAddress("execute-eventregistration"), new
            {
                context.Message.ParticipantEmailAddress,
                context.Message.ParticipantLicenseNumber,
                context.Message.ParticipantCategory,
                context.Message.EventId,
                context.Message.RaceId
            });

            var paymentInfo = _paymentInfoService.GetPaymentInfo(context.Message.ParticipantEmailAddress, context.Message.CardNumber);

            builder.AddActivity("ProcessPayment", context.GetDestinationAddress("execute-processpayment"), paymentInfo);

            builder.AddSubscription(context.SourceAddress, RoutingSlipEvents.ActivityFaulted, RoutingSlipEventContents.None, "ProcessPayment",
                x => x.Send<RegistrationPaymentFailed>(new
                {
                    context.Message.SubmissionId
                }));


            builder.AddSubscription(context.SourceAddress, RoutingSlipEvents.Completed, x => x.Send<RegistrationCompleted>(new
            {
                context.Message.SubmissionId
            }));


            return builder.Build();
        }
    }
}