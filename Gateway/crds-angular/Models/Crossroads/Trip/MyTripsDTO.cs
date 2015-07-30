using System.Collections.Generic;
using AutoMapper;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class MyTripsDTO
    {
        [JsonProperty(PropertyName = "myTrips")]
        public List<Trip> MyTrips { get; set; }

        public MyTripsDTO()
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

    public class TripGift
    {
        public string DonorNickname { get; set; }
        public string DonorFirstName { get; set; }
        public string DonorLastName { get; set; }
        public string DonorEmail { get; set; }
        public int DonationAmount { get; set; }
        public string DonationDate { get; set; }
    }
}