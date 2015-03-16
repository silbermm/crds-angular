using System;
using System.Collections.Generic;

namespace MinistryPlatform.Models
{
    public class Opportunity
    {
        public int OpportunityId { get; set; }
        public string OpportunityName { get; set; }
        //public DateTime Opportunity_Date { get; set; }
        public string EventType { get; set; }
        //public int EventTypeId { get; set; }
        public List<Event> Events { get; set; }
        
    }
}
