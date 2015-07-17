using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Models.Json;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public class StripeBalanceTransaction : StripeObject
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("net")]
        public int Net { get; set; }

        [JsonProperty("fee")]
        public int? Fee { get; set; }
    }
}