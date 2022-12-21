namespace Registration.Tests;

using Components.StateMachines;
using Contracts;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;


[TestFixture]
public class When_a_registration_is_received
{
    [Test]
    public async Task Should_produce_the_process_registration_event()
    {
        await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness(x =>
            {
                x.AddSagaStateMachine<RegistrationStateMachine, RegistrationState>();
            })
            .BuildServiceProvider(true);

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
        
        Assert.That(await harness.Published.Any<ProcessRegistration>(), Is.True);
    }
}