using System;

namespace crds_angular.Models.Crossroads
{
    public class ServingOpportunity
    {
        public string OpportunityName { get; set; }
        //public DateTime OpportunityDateTime { get; set; }
        public ServingRSVP RSVP { get; set; } 
    }
}