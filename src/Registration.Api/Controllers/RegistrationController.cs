namespace Registration.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Routing;
    using Contracts;
    using Data;
    using Data.Models;
    using MassTransit;
    using Models;


    public class RegistrationController :
        ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new[] {"value1", "value2"};
        }

        // GET api/<controller>/5
        [Route("~/api/registration/{submissionId:Guid}", Name = "RegistrationStatus")]
        public async Task<RegistrationModel> Get(Guid submissionId)
        {
            var reader = new RegistrationStateReader(ConfigurationManager.AppSettings["DatabaseConnectionString"]);
            try
            {
                var registration = await reader.Get(submissionId);

                return registration;
            }
            catch (Exception exception)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        // POST api/<controller>
        public async Task<HttpResponseMessage> Post([FromBody] RegistrationModel registration)
        {
            await WebApiApplication.Bus.Send<SubmitRegistration>(registration);


            var urlHelper = new UrlHelper(Request);

            var response = new RegistrationResponseModel
            {
                SubmissionId = registration.SubmissionId,
                Actions = new Dictionary<string, Uri>
                {
                    {"Status", new Uri(urlHelper.Link("RegistrationStatus", new {submissionId = registration.SubmissionId}))}
                }
            };

            return Request.CreateResponse(HttpStatusCode.Accepted, response);
        }

//        // PUT api/<controller>/5
//        }

//
//        public void Put(int id, [FromBody]string value)

//        {
//        // DELETE api/<controller>/5
//        public void Delete(int id)
//        {
//        }
    }
}