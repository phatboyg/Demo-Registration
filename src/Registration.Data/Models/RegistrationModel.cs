namespace Registration.Data.Models
{
    using System;
    using Contracts;


    public class RegistrationModel :
        SubmitRegistration
    {
        public Guid SubmissionId { get; set; }
        public DateTime Timestamp { get; set; }
        public string ParticipantEmailAddress { get; set; }
        public string ParticipantLicenseNumber { get; set; }
        public string ParticipantCategory { get; set; }
        public string EventId { get; set; }
        public string RaceId { get; set; }
        public string Status { get; set; }
        public string CardNumber { get; set; }
    }
}