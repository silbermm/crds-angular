using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class CheckScannerCheck
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("check_number")]
        public string CheckNumber { get; set; }
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty(PropertyName = "check_date", ItemConverterType = typeof(JavaScriptDateTimeConverter))]
        public DateTime CheckDate { get; set; }
        [JsonProperty(PropertyName = "scan_date", ItemConverterType = typeof(JavaScriptDateTimeConverter))]
        public DateTime ScanDate { get; set; }

        [JsonProperty("name_1")]
        public string Name1 { get; set; }
        [JsonProperty("name_2")]
        public string Name2 { get; set; }
        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("routing_number")]
        public string RoutingNumber { get; set; }
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("donation_id")]
        public int DonationId { get; set; }
    }

    public class Address
    {
        [JsonProperty("line_1")]
        public string Line1 { get; set; }
        [JsonProperty("line_2")]
        public string Line2 { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }
    }
}