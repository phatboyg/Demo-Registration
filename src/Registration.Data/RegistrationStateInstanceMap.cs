namespace Registration.Data
{
    using Components.StateMachines;
    using MassTransit.EntityFrameworkCoreIntegration.Mappings;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;


    public class RegistrationStateInstanceMap :
        SagaClassMap<RegistrationStateInstance>
    {
        protected override void Configure(EntityTypeBuilder<RegistrationStateInstance> entity, ModelBuilder model)
        {
            entity.Property(x => x.ParticipantEmailAddress)
                .HasMaxLength(256);
            entity.Property(x => x.ParticipantCategory)
                .HasMaxLength(20);
            entity.Property(x => x.ParticipantLicenseNumber)
                .HasMaxLength(20);
            entity.Property(x => x.EventId)
                .HasMaxLength(60);
            entity.Property(x => x.RaceId)
                .HasMaxLength(60);
        }
    }
}