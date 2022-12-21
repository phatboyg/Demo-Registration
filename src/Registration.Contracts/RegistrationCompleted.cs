namespace Registration.Contracts;

using System;
using System.Collections.Generic;
using MassTransit.Courier.Contracts;


public record RegistrationCompleted :
    RoutingSlipCompleted
{
    public Guid SubmissionId { get; init; }
    public Guid TrackingNumber { get; init; }

    public DateTime Timestamp { get; set; }
    public TimeSpan Duration { get; set; }
    public IDictionary<string, object> Variables { get; set; }
}