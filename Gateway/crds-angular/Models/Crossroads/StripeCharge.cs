using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public class StripeCharge : StripeObject
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}