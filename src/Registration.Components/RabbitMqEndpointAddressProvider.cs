namespace Registration.Components;

using System;
using MassTransit;


public class RabbitMqEndpointAddressProvider :
    IEndpointAddressProvider
{
    readonly IEndpointNameFormatter _formatter;

    public RabbitMqEndpointAddressProvider(IEndpointNameFormatter formatter)
    {
        _formatter = formatter;
    }

    public Uri GetExecuteEndpoint<T, TArguments>()
        where T : class, IExecuteActivity<TArguments>
        where TArguments : class
    {
        return new Uri($"exchange:{_formatter.ExecuteActivity<T, TArguments>()}");
    }
}