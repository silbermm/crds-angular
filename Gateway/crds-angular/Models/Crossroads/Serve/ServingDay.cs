using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Serve
{
    public class ServingDay
    {
        [JsonProperty(PropertyName = "day")]
        public string Day { get; set; }

        [JsonIgnore]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "serveTimes")]
        public List<ServingTime> ServeTimes { get; set; }

        public ServingDay()
        {
            this.ServeTimes = new List<ServingTime>();
        }
    }
}