using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class CheckScannerCheck
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("donorId")]
        public int? DonorId { get; set; }
        [JsonProperty("checkNumber")]
        public string CheckNumber { get; set; }
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty(PropertyName = "checkDate", NullValueHandling = NullValueHandling.Ignore), JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? CheckDate { get; set; }
        [JsonProperty(PropertyName = "scanDate", NullValueHandling = NullValueHandling.Ignore), JsonConverter(typeof(IsoDateTimeConverter))]
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

        [JsonProperty("exported")]
        public bool Exported { get; set; }

        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }


        #region Check Errors properties and accessors
        [JsonIgnore]
        private string AccountNumberLastFour { get { return AccountNumber.Substring(AccountNumber.Length - 4); } }
        [JsonIgnore]
        public string EmailError
        {
            get
            {
                return string.Format("Routing Number: {0}\nAccount Number Last Four: {1}\nError: {2}\n\n", RoutingNumber, AccountNumberLastFour, Error);
            }
        }
        #endregion
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