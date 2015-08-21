using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class StripeRefund
    {
        [JsonProperty("data")]
        public List<StripeRefundData> Data { get; set; }
    }

    public class StripeRefundData
    {
        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("charge")]
        public string Charge { get; set; }
    }
        
    }