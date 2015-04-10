using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public class ServeResponseDto
    {
        [JsonProperty(PropertyName = "contactid")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public long EndDateUnix { get; set; }

        [JsonProperty(PropertyName = "eventTypeId")]
        public int EventTypeId { get; set; }

        [JsonProperty(PropertyName = "opportunityId")]
        public int OpportunityId { get; set; }

        [JsonProperty(PropertyName = "startDate")]
        public long StartDateUnix { get; set; }
    }
}