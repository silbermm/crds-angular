using System.Collections.Generic;

namespace crds_angular.Models.Crossroads
{
    public class ServingTeam
    {
        public string TeamName { get; set; }
        public List<GroupOpportunity> Opportunities { get; set; }

        public ServingTeam()
        {
            Opportunities = new List<GroupOpportunity>( );
        }
    }
}