using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Serve
{
    public class SaveRsvpDto
    {
        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public long EndDateUnix { get; set; }

        [JsonProperty(PropertyName = "eventTypeId")]
        public int EventTypeId { get; set; }

        [JsonProperty(PropertyName = "opportunityId")]
        public int OpportunityId { get; set; }

        [JsonProperty(PropertyName = "startDate")]
        public long StartDateUnix { get; set; }

        [JsonProperty(PropertyName = "signUp")]
        public bool SignUp { get; set; }

        [JsonProperty(PropertyName = "alternateWeeks")]
        public bool AlternateWeeks { get; set; }
    }
}