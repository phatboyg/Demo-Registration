namespace Registration.Data
{
    using MassTransit.EntityFrameworkIntegration;
    using RegistrationState;


    public class RegistrationStateInstanceMap :
        SagaClassMapping<RegistrationStateInstance>
    {
        public RegistrationStateInstanceMap()
        {
            Property(x => x.ParticipantEmailAddress)
                .HasMaxLength(256);
            Property(x => x.ParticipantCategory)
                .HasMaxLength(20);
            Property(x => x.ParticipantLicenseNumber)
                .HasMaxLength(20);
            Property(x => x.EventId)
                .HasMaxLength(60);
            Property(x => x.RaceId)
                .HasMaxLength(60);
        }
    }
}