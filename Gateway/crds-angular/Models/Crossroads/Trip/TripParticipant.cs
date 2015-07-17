using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class TripParticipant
    {
        [JsonProperty(PropertyName = "tripParticipantId")]
        public int EventParticipantId { get; set; }

        [JsonProperty(PropertyName = "trip")]
        public Trip Trip { get; set; }
       
    }
}