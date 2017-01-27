namespace Registration.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Contracts;
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
        public string Get(Guid id)
        {
            return "value";
        }

        // POST api/<controller>
        public async Task<HttpResponseMessage> Post([FromBody] RegistrationModel registration)
        {
            await WebApiApplication.Bus.Send<SubmitRegistration>(registration);

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

//        {
//        public void Put(int id, [FromBody]string value)

//        // PUT api/<controller>/5
//        }
//
//        // DELETE api/<controller>/5
//        public void Delete(int id)
//        {
//        }
    }
}