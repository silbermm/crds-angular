using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class Trip
    {
        [JsonIgnore]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "eventParticipantId")]
        public int EventParticipantId { get; set; }

        [JsonIgnore]
        public string EventType { get; set; }

        [JsonProperty(PropertyName = "tripStartDate")]
        public string EventStartDate { get; set; }

        [JsonProperty(PropertyName = "tripEnd")]
        public string EventEndDate { get; set; }

        [JsonProperty(PropertyName = "tripName")]
        public string EventTitle { get; set; }

        [JsonProperty(PropertyName = "fundraisingDays")]
        public int FundraisingDaysLeft { get; set; }

        [JsonProperty(PropertyName = "fundraisingGoal")]
        public int FundraisingGoal { get; set; }

        [JsonProperty(PropertyName = "totalRaised")]
        public int TotalRaised { get; set; }

        [JsonProperty(PropertyName = "tripGifts")]
        public List<TripGift> TripGifts { get; set; }

        public Trip()
        {
            TripGifts = new List<TripGift>();
        }
    }
}