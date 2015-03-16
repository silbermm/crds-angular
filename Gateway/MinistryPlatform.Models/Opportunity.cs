using System.Collections.Generic;

namespace MinistryPlatform.Models
{
    public class Opportunity
    {
        public int OpportunityId { get; set; }
        public string OpportunityName { get; set; }
<<<<<<< HEAD
        //public DateTime Opportunity_Date { get; set; }
        public string EventType { get; set; }
        //public int EventTypeId { get; set; }
        public List<Event> Events { get; set; }
        
=======
        public int EventTypeId { get; set; }
        public List<Event> Events { get; set; }
>>>>>>> ae18c05b97309e38e47de93528a2575394315cca
    }
}
