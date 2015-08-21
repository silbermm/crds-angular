using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class CheckScannerCheck
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("checkNumber")]
        public string CheckNumber { get; set; }
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty(PropertyName = "checkDate"), JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? CheckDate { get; set; }
        [JsonProperty(PropertyName = "scanDate"), JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? ScanDate { get; set; }

        [JsonProperty("name1")]
        public string Name1 { get; set; }
        [JsonProperty("name2")]
        public string Name2 { get; set; }
        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("routingNumber")]
        public string RoutingNumber { get; set; }
        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }

        [JsonProperty("donationId")]
        public int DonationId { get; set; }
    }

    public class Address
    {
        [JsonProperty("line1")]
        public string Line1 { get; set; }
        [JsonProperty("line2")]
        public string Line2 { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }
    }
}