using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Opportunity
{
    public class OpportunityGroup
    {
        [JsonProperty(PropertyName = "groupId")]
        public int GroupId { get; set; }

        [JsonProperty(PropertyName = "groupName")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "eventTypeId")]
        public int EventTypeId { get; set; }

        public IList<OpportunityGroupParticipant> Participants { get; set; }
    }
}