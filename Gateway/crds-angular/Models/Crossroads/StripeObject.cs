using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public abstract class StripeObject
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public bool Object { get; set; }

        [JsonProperty("livemode")]
        public bool LiveMode { get; set; }
    }
}