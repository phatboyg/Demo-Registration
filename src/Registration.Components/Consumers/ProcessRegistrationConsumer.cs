namespace Registration.Components.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Activities.EventRegistration;
    using Activities.LicenseVerification;
    using Activities.ProcessPayment;
    using Contracts;
    using MassTransit;
    using MassTransit.Courier;
    using MassTransit.Courier.Contracts;
    using Microsoft.Extensions.Logging;
    using Services;


    public class ProcessRegistrationConsumer :
        IConsumer<ProcessRegistration>
    {
        readonly IEndpointNameFormatter _endpointNameFormatter;
        readonly ILogger<ProcessRegistrationConsumer> _logger;
        readonly ISecurePaymentInfoService _paymentInfoService;

        public ProcessRegistrationConsumer(ILogger<ProcessRegistrationConsumer> logger, IEndpointNameFormatter endpointNameFormatter)
        {
            _logger = logger;
            _endpointNameFormatter = endpointNameFormatter;
            _paymentInfoService = new SecurePaymentInfoService();
        }

        public async Task Consume(ConsumeContext<ProcessRegistration> context)
        {
            _logger.LogInformation("Processing registration: {0} ({1})", context.Message.SubmissionId, context.Message.ParticipantEmailAddress);

            var routingSlip = CreateRoutingSlip(context);

            await context.Execute(routingSlip).ConfigureAwait(false);
        }

        RoutingSlip CreateRoutingSlip(ConsumeContext<ProcessRegistration> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());

            if (!string.IsNullOrWhiteSpace(context.Message.ParticipantLicenseNumber))
            {
                builder.AddActivity("LicenseVerification", GetActivityAddress<LicenseVerificationActivity, LicenseVerificationArguments>(), new
                {
                    LicenseNumber = context.Message.ParticipantLicenseNumber,
                    EventType = "Road",
                    Category = context.Message.ParticipantCategory
                });

                builder.AddSubscription(context.SourceAddress, RoutingSlipEvents.ActivityFaulted, RoutingSlipEventContents.None, "LicenseVerification",
                    x => x.Send<RegistrationLicenseVerificationFailed>(new
                    {
                        context.Message.SubmissionId
                    }));
            }

            builder.AddActivity("EventRegistration", GetActivityAddress<EventRegistrationActivity, EventRegistrationArguments>(), new
            {
                context.Message.ParticipantEmailAddress,
                context.Message.ParticipantLicenseNumber,
                context.Message.ParticipantCategory,
                context.Message.EventId,
                context.Message.RaceId
            });

            var paymentInfo = _paymentInfoService.GetPaymentInfo(context.Message.ParticipantEmailAddress, context.Message.CardNumber);

            builder.AddActivity("ProcessPayment", GetActivityAddress<ProcessPaymentActivity, ProcessPaymentArguments>(), paymentInfo);

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

        Uri GetActivityAddress<TActivity, TArguments>()
            where TActivity : class, IExecuteActivity<TArguments>
            where TArguments : class
        {
            var name = _endpointNameFormatter.ExecuteActivity<TActivity, TArguments>();

            return new Uri($"exchange:{name}");
        }
    }
}