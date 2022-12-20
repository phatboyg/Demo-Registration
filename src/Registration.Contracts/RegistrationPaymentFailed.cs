namespace Registration.Contracts;

using System;
using MassTransit;


public record RegistrationPaymentFailed
{
    public Guid SubmissionId { get; init; }

    public ExceptionInfo ExceptionInfo { get; init; }
}