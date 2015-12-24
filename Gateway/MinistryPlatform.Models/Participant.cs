using System;

namespace MinistryPlatform.Models
{
    public class Participant
    {
        public int ContactId { get; set; }
        public int ParticipantId { get; set;}
        public string EmailAddress { get; set; }
        public string PreferredName { get; set; }
        public string DisplayName { get; set; }
        public int Age { get; set; }
        public DateTime? ParticipantStart { get; set; }
        public DateTime? AttendanceStart { get; set; }
    }
}
