using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public abstract class StripeObject
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public dynamic Object { get; set; }

        [JsonProperty("livemode")]
        public bool LiveMode { get; set; }
    }
}