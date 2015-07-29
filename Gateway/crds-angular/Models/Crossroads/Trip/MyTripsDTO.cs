using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class MyTripsDTO
    {
        [JsonProperty(PropertyName = "myTrips")]
        public List<Trip> MyTrips { get; set; }

        MyTripsDTO()
        {
            MyTrips = new List<Trip>();
        }

    }
    public class Trip
    {
        [JsonIgnore]
        public int EventId { get; set; }

        [JsonIgnore]
        public string EventType { get; set; }

        [JsonProperty(PropertyName = "tripParticipantId")]
        public int EventParticipantId { get; set; }

        [JsonProperty(PropertyName = "tripStartDate")]
        public long EventStartDate { get; set; }

        [JsonProperty(PropertyName = "tripEnd")]
        public string EventEndDate { get; set; }

        [JsonProperty(PropertyName = "tripName")]
        public string EventTitle { get; set; }

        [JsonProperty(PropertyName = "tripGifts")]
        public List<TripGift> TripGifts { get; set; } 
    }

    public class TripGift
    {
        public string DonorFirstName { get; set; }
        public string DonorLastName { get; set; }
        public string DonorEmail { get; set; }
        public decimal DonationAmount { get; set; }
        public string DonationDate { get; set; }
    }
}