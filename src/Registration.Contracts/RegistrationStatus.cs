namespace Registration.Contracts;

using System;


public record RegistrationStatus :
    RegistrationDetail
{
    public string Status { get; init; }
    public DateTime? ParticipantLicenseExpirationDate { get; init; }
}