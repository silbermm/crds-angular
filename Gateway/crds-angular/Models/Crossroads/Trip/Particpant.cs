using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class Particpant
    {
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

        [JsonProperty(PropertyName = "tripParticipant")]
        public TripParticipant TripParticipant { get; set; }
    }
}