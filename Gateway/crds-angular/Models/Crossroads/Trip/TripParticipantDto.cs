using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class TripParticipantDto
    {
        public TripParticipantDto()
        {
            Trips = new List<TripDto>();
        }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonIgnore]
        public string Lastname { get; set; }

        [JsonIgnore]
        public string Nickname { get; set; }

        [JsonProperty(PropertyName = "participantId")]
        public int ParticipantId { get; set; }

        [JsonProperty(PropertyName = "participantName")]
        public string ParticipantName
        {
            get { return string.Format("{0} {1}", Nickname, Lastname); }
        }

        [JsonProperty(PropertyName = "showGiveButton")]
        public bool ShowGiveButton { get; set; }

        [JsonProperty(PropertyName = "showShareButtons")]
        public bool ShowShareButtons { get; set; }

        [JsonProperty(PropertyName = "participantPhotoUrl")]
        public string PhotoUrl
        {
            get { return "http://crossroads-media.imgix.net/images/avatar.svg"; }
        }

        [JsonProperty(PropertyName = "trips")]
        public List<TripDto> Trips { get; set; }
    }
}
