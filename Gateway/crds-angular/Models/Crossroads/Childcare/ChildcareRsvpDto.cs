using System.Collections.Generic;

namespace crds_angular.Models.Crossroads.Childcare
{
    public class ChildcareRsvpDto
    {
        public int EventId { get; set; }
        public List<int> Participants { get; set; }
    }
}