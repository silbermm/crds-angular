using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public class ServeResponseDto
    {
        [JsonProperty(PropertyName = "contactid")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public DateTime EndDate { get; set; }

        [JsonProperty(PropertyName = "eventTypeId")]
        public int EventTypeId { get; set; }

        [JsonProperty(PropertyName = "opportunityId")]
        public int OpportunityId { get; set; }

        [JsonProperty(PropertyName = "startDate")]
        public DateTime StartDate { get; set; }
    }
}