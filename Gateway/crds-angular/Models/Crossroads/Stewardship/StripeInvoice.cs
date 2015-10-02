using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class StripeInvoice : StripeObject
    {
        [JsonProperty("id")]
        public string InvoiceId { get; set; }

        [JsonProperty("subscription")]
        public string Subscription { get; set; }

        [JsonProperty("customer")]
        public string Customer { get; set; }

        [JsonProperty("charge")]
        public string Charge { get; set; }

        [JsonProperty("amount_due")]
        public int Amount { get; set; }
    }
}
