using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.MP
{
    public class Household
    {
        [JsonProperty(PropertyName = "addressLine1")]
        public string Address_Line_1 { get; set; }

        [JsonProperty(PropertyName = "addressLine2")]
        public string Address_Line_2 { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "postalCode")]
        public string Postal_Code { get; set; }

        [JsonProperty(PropertyName = "homePhone")]
        public string Home_Phone { get; set; }

        [JsonProperty(PropertyName = "foreignCountry")]
        public string Foreign_Country { get; set; }

        [JsonProperty(PropertyName = "county")]
        public string County { get; set; }

        [JsonProperty(PropertyName = "congregationId")]
        public int? Congregation_ID { get; set; }

        [JsonProperty(PropertyName = "householdId")]
        public int Household_ID { get; set; }

        [JsonProperty(PropertyName = "members")]
        public List<HouseholdMember> Household_Members { get; set; }

        public Household()
        {
            Household_Members = new List<HouseholdMember>();
        }
    }
}