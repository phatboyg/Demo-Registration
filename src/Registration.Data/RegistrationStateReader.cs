namespace Registration.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Components.StateMachines;
    using Microsoft.EntityFrameworkCore;
    using Models;


    public class RegistrationStateReader :
        IRegistrationStateReader
    {
        readonly RegistrationDbContext _context;

        public RegistrationStateReader(RegistrationDbContext context)
        {
            _context = context;
        }

        public async Task<RegistrationModel> Get(Guid submissionId)
        {
            {
                var instance = await _context.Set<RegistrationStateInstance>()
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