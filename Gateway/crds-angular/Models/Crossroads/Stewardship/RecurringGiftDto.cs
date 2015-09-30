using System;
using Newtonsoft.Json;

namespace crds_angular.Controllers.API
{
    public class RecurringGiftDto
    {
        [JsonProperty(PropertyName = "stripe_token_id")]
        public string StripeTokenId { get; set; }
        [JsonProperty(PropertyName = "amount")]
        public decimal PlanAmount { get; set; }
        [JsonProperty(PropertyName = "program")]
        public string Program { get; set; }
        [JsonProperty(PropertyName = "interval")]
        public string PlanInterval { get; set; }
        [JsonProperty(PropertyName = "start_date")]
        public DateTime StartDate { get; set; }
    }
}
