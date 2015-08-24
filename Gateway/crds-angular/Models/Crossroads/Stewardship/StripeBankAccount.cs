using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class StripeBankAccount : StripeObject
    {
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("routing_number")]
        public string RoutingNumber { get; set; }
    }
}