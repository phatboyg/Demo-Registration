namespace Registration.Contracts;

using System;


public record RetryDelayExpired(Guid RegistrationId);