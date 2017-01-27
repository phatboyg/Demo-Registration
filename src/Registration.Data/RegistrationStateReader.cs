namespace Registration.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using MassTransit.EntityFrameworkIntegration;
    using Models;
    using RegistrationState;


    public class RegistrationStateReader :
        IRegistrationStateReader
    {
        readonly SagaDbContextFactory _sagaDbContextFactory;

        public RegistrationStateReader(string connectionString)
        {
            _sagaDbContextFactory = () => new SagaDbContext<RegistrationStateInstance, RegistrationStateInstanceMap>(connectionString);
        }

        public async Task<RegistrationModel> Get(Guid submissionId)
        {
            using (var dbContext = _sagaDbContextFactory())
            {
                var instance = await dbContext.Set<RegistrationStateInstance>()
                    .Where(x => x.CorrelationId == submissionId)
                    .SingleAsync().ConfigureAwait(false);

                return new RegistrationModel
                {
                    SubmissionId = instance.CorrelationId,
                    ParticipantEmailAddress = instance.ParticipantEmailAddress,
                    ParticipantCategory = instance.ParticipantCategory,
                    ParticipantLicenseNumber = instance.ParticipantLicenseNumber,
                    EventId = instance.EventId,
                    RaceId = instance.RaceId,
                    Status = instance.CurrentState
                };
            }
        }
    }
}