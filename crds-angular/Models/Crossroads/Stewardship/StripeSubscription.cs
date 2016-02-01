using System;
using crds_angular.Models.Json;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class StripeSubscription
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("Plan")]
        public StripePlan Plan { get; set; }

        [JsonProperty("start")]
        public string Start { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("customer")]
        public string Customer { get; set; }

        [JsonProperty("current_period_start")]
        public string CurrentPeriodStart { get; set; }

        [JsonProperty("current_period_end")]
        public string CurrentPeriodEnd { get; set; }

        [JsonProperty("ended_at")]
        public string EndedAt { get; set; }

        [JsonProperty("trial_end")]
        [JsonConverter(typeof(StripeDateTimeConverter))]
        public DateTime? TrialEnd { get; set; }
    }
}