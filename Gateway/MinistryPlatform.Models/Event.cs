using System;
using System.Collections.Generic;

namespace MinistryPlatform.Models
{
    public class Event
    {
        private IList<int> participants = new List<int>();

        public int EventId { get; set; }
        public string EventTitle { get; set; }
        public string EventType { get; set; }
        //public int EventTypeId { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }

        public IList<int> Participants
        {
            get { return (participants); }
        }
    }
}
