using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public class StripeCharge : StripeObject
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("failure_code")]
        public string FailureCode { get; set; }

        [JsonProperty("failure_message")]
        public string FailureMessage { get; set; }
    }
}