using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class StripeCharges
    {
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        [JsonProperty("has_more")]
        public bool HasMore { get; set; }

        [JsonProperty("data")]
        public List<StripeCharge> Data { get; set; }
    }
}