using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class StripeTransfer : StripeObject
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }
    }
}