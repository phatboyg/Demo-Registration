namespace Registration.Data
{
    using System;
    using System.Threading.Tasks;
    using Models;


    public interface IRegistrationStateReader
    {
        Task<RegistrationModel> Get(Guid submissionId);
    }
}