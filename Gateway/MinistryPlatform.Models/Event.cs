using System;

namespace MinistryPlatform.Models
{
    public class Event
    {
        public string EventTitle { get; set; }
        public string EventType { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
    }
}
