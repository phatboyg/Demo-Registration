namespace Registration.Components.Activities;

using System;


public record ProcessPaymentLog
{
    /// <summary>
    /// The date the charge was processed
    /// </summary>
    public DateTime ChargeDate { get; init; }

    /// <summary>
    /// The authorization code received from the payment provider
    /// </summary>
    public string AuthorizationCode { get; init; }

    /// <summary>
    /// The amount charged
    /// </summary>
    public decimal Amount { get; init; }
}