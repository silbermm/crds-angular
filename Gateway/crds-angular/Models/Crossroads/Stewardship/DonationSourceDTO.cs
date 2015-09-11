using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class DonationSourceDTO
    {
        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore), JsonConverter(typeof(StringEnumConverter))]
        public PaymentType SourceType { get; set; }

        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "last4", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountNumberLast4 { get; set; }

        [JsonProperty(PropertyName = "brand", NullValueHandling = NullValueHandling.Ignore), JsonConverter(typeof(StringEnumConverter))]
        public CreditCardType? CardType { get; set; }

        [JsonProperty(PropertyName = "payment_processor_id", NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentProcessorId { get; set; }
    }
}