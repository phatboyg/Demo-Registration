namespace Registration.Tests;

using Components;
using Contracts;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;


[TestFixture]
public class When_a_payment_activity
{
    [Test]
    public async Task Completes_should_complete_the_routing_slip()
    {
        await using var provider = BuildServiceProvider();

        var harness = provider.GetTestHarness();

        await harness.Start();

        var submissionId = NewId.NextGuid();

        await harness.Bus.Publish(new RegistrationReceived
        {
            SubmissionId = submissionId,
            ParticipantEmailAddress = "frank@furter.com",
            ParticipantLicenseNumber = "2112",
            ParticipantCategory = "CAT6",
            CardNumber = "4242",
            EventId = "EVENT123",
            RaceId = "Novice"
        });

        Assert.That(await harness.Consumed.Any<RegistrationCompleted>(x => x.Exception is null), Is.True);
        
        Response<RegistrationStatus> response = await harness.GetRequestClient<GetRegistrationStatus>()
            .GetResponse<RegistrationStatus>(new { submissionId });

        Assert.That(response.Message.Status, Is.EqualTo("Registered"));
        
        Assert.That(response.Message.RegistrationId.HasValue, Is.True);
    }

    [Test]
    public async Task Faults_should_complete_the_routing_slip()
    {
        await using var provider = BuildServiceProvider();

        var harness = provider.GetTestHarness();

        await harness.Start();

        var submissionId = NewId.NextGuid();

        await harness.Bus.Publish(new RegistrationReceived
        {
            SubmissionId = submissionId,
            ParticipantEmailAddress = "frank@furter.com",
            ParticipantLicenseNumber = "2112",
            ParticipantCategory = "CAT6",
            CardNumber = "4147",
            EventId = "EVENT123",
            RaceId = "Novice"
        });

        Assert.That(await harness.Consumed.Any<RegistrationPaymentFailed>(x => x.Exception is null), Is.True);
    }

    static ServiceProvider BuildServiceProvider()
    {
        return new ServiceCollection()
            .AddMassTransitTestHarness(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.AddConsumersFromNamespaceContaining<ComponentsNamespace>();
                x.AddActivitiesFromNamespaceContaining<ComponentsNamespace>();
                x.AddSagaStateMachinesFromNamespaceContaining<ComponentsNamespace>();
            })
            .BuildServiceProvider(true);
    }
}