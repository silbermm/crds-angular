using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Childcare
{
    public class ChildcareRsvpDto
    {
        [JsonProperty(PropertyName = "eventId")]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "participants")]
        public List<int> Participants { get; set; }
    }
}