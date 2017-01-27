namespace Registration.Api.Models
{
    using System;
    using System.Collections.Generic;


    public class RegistrationResponseModel
    {
        public Guid SubmissionId { get; set; }
        public Dictionary<string,Uri> Actions { get; set; }
    }
}