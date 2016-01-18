using System;
using System.Collections.Generic;

namespace MinistryPlatform.Models
{
    public class Opportunity
    {
        public int OpportunityId { get; set; }
        public string OpportunityName { get; set; }
        public string EventType { get; set; }
        public int EventTypeId { get; set; }
        public List<Event> Events { get; set; }
        public string RoleTitle { get; set; }
        //public int Capacity { get; set; }
        public int? MaximumNeeded { get; set; }
        public int? MinimumNeeded { get; set; }
        public List<Response> Responses { get; set; }
        public string Room { get; set; }
        public string GroupContactName { get; set; }
        public int GroupContactId { get; set; }
        public string GroupName { get; set; }
        public int GroupId { get; set; }
        public TimeSpan? ShiftStart { get; set; }
        public TimeSpan? ShiftEnd { get; set; }

    }
}
