using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class StripeToken : StripeObject
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}