using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class MyTripsDto
    {
        [JsonProperty(PropertyName = "myTrips")]
        public List<Trip> MyTrips { get; set; }

        public MyTripsDto()
        {
            MyTrips = new List<Trip>();
        }

    }
}