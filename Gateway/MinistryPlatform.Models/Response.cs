using System;

namespace MinistryPlatform.Models
{
    public class Response
    {
        public int Response_ID { get; set; }
        public DateTime Response_Date { get; set; }
        public int Opportunity_ID { get; set; }
        public int Participant_ID { get; set; }
        public bool Closed { get; set; }
        public string Comments { get; set; }
        public int? Response_Result_ID { get; set; }
        public int Event_ID { get; set; }
        //public DateTime Opportunity_Date { get; set; }
    }
}
