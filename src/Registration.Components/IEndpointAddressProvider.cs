namespace Registration.Components;

using System;
using MassTransit;


public interface IEndpointAddressProvider
{
    Uri GetExecuteEndpoint<T, TArguments>()
        where T : class, IExecuteActivity<TArguments>
        where TArguments : class;
}