namespace Registration.Components.Activities;

using System;


public interface ProcessPaymentLog
{
    /// <summary>
    /// The date the charge was processed
    /// </summary>
    DateTime ChargeDate { get; }

    /// <summary>
    /// The authorization code received from the payment provider
    /// </summary>
    string AuthorizationCode { get; }

    /// <summary>
    /// The amount charged
    /// </summary>
    decimal Amount { get; }
}