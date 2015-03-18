using System.Collections.Generic;

namespace crds_angular.Models.Crossroads
{
    public class GroupOpportunity
    {
        //should OpportunityName be called Role?  tm 3/16
        public string OpportunityName { get; set; }
        //public DateTime OpportunityDateTime { get; set; }
        public List<ServeOccurance> ServeOccurances { get; set; }
        public ServingRSVP Rsvp { get; set; } 
    }
}