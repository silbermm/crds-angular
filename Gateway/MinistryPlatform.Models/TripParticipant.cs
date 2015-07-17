using System;

namespace MinistryPlatform.Models
{
    public class TripParticipant
    {
        public string EmailAddress { get; set; }
        public DateTime EventEndDate { get; set; }
        public int EventId { get; set; }
        public int EventParticipantId { get; set; }
        public DateTime EventStartDate { get; set; }
        public string EventTitle { get; set; }
        public string EventType { get; set; }
        public string Lastname { get; set; }
        public string Nickname { get; set; }
    }
}