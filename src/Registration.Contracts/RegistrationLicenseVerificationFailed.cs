namespace Registration.Contracts;

using System;
using MassTransit;


public record RegistrationLicenseVerificationFailed
{
    public Guid SubmissionId { get; init; }

    public ExceptionInfo ExceptionInfo { get; init; }
}