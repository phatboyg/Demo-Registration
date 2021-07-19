namespace Registration.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Contracts;
    using Data;
    using Data.Models;
    using MassTransit;
    using Microsoft.AspNetCore.Mvc;


    [ApiController]
    [Route("[controller]")]
    public class RegistrationController :
        ControllerBase
    {
        readonly IPublishEndpoint _publishEndpoint;
        readonly IRegistrationStateReader _reader;

        public RegistrationController(IRegistrationStateReader reader, IPublishEndpoint publishEndpoint)
        {
            _reader = reader;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet(Name = "RegistrationStatus")]
        public async Task<IActionResult> Get(Guid submissionId)
        {
            try
            {
                var registration = await _reader.Get(submissionId);

                return Ok(registration);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegistrationModel registration)
        {
            await _publishEndpoint.Publish<SubmitRegistration>(registration);

            var response = new
            {
                registration.SubmissionId,
                Actions = new Dictionary<string, string>
                {
                    {"Status", Url.Link("RegistrationStatus", new {submissionId = registration.SubmissionId})}
                }
            };

            return Ok(response);
        }
    }
}