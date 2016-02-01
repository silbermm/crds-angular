using System.Collections.Generic;
using Newtonsoft.Json;

namespace MinistryPlatform.Models.DTO
{
    public class RespondToOpportunityDto
    {
        [JsonProperty(PropertyName = "opportunityId")]
        public int OpportunityId { get; set; }

        [JsonProperty(PropertyName = "participants")]
        public List<int> Participants { get; set; }
    }
}
