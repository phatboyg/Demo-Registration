namespace Registration.Api.Controllers;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("[controller]")]
public class RegistrationController :
    ControllerBase
{
    /// <summary>
    /// Return the registration detail, including status
    /// </summary>
    /// <param name="submissionId">The registration submission id</param>
    /// <param name="client"></param>
    /// <returns></returns>
    [HttpGet("{submissionId}", Name = "RegistrationStatus")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid submissionId, [FromServices] IRequestClient<GetRegistrationStatus> client)
    {
        try
        {
            Response<RegistrationStatus> response = await client.GetResponse<RegistrationStatus>(new GetRegistrationStatus { SubmissionId = submissionId });

            var registration = response.Message;

            return Ok(registration);
        }
        catch (RequestFaultException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Submits an order
    /// <response code="202">The registration has been accepted but not yet completed</response>
    /// </summary>
    /// <param name="registration">The registration detail</param>
    /// <param name="publishEndpoint"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Post([FromBody] RegistrationDetail registration, [FromServices] IPublishEndpoint publishEndpoint)
    {
        await publishEndpoint.Publish<SubmitRegistration>(registration);

        var response = new
        {
            registration.SubmissionId,
            Actions = new Dictionary<string, string> { { "Status", Url.Link("RegistrationStatus", new { submissionId = registration.SubmissionId }) } }
        };

        return Ok(response);
    }
}