using System;
using Newtonsoft.Json;
using crds_angular.Models.Json;

namespace crds_angular.Models.Crossroads
{
    public class StripeEvent : StripeObject
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("created")]
        [JsonConverter(typeof(StripeDateTimeConverter))]
        public DateTime? Created { get; set; }

        [JsonProperty("data")]
        public StripeEventData Data { get; set; }
    }

    public class StripeEventData
    {
        [JsonProperty("object")]
        public dynamic Object { get; set; }
    }
}