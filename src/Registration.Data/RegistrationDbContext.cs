namespace Registration.Data;

using System.Collections.Generic;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;


public class RegistrationDbContext :
    SagaDbContext
{
    public RegistrationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override IEnumerable<ISagaClassMap> Configurations => new[] { new RegistrationStateInstanceMap() };
}