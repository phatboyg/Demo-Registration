namespace Registration.Components.Activities;

using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;


public class AssignWaiverActivity :
    IExecuteActivity<AssignWaiverArguments>
{
    readonly ILogger<AssignWaiverActivity> _logger;

    public AssignWaiverActivity(ILogger<AssignWaiverActivity> logger)
    {
        _logger = logger;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<AssignWaiverArguments> context)
    {
        var arguments = context.Arguments;

        var emailAddress = arguments.ParticipantEmailAddress;

        _logger.LogInformation("Assigning waiver to: {Email}", emailAddress);

        await Task.Delay(10);

        if (emailAddress == "joey@friends.tv")
            throw new RoutingSlipException($"The document server failed to respond: {emailAddress}");


        return context.Completed();
    }
}