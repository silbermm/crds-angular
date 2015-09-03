using System;
using Newtonsoft.Json;

namespace crds_angular.Models.MP
{
    public class HouseholdMember
    {

        [JsonProperty(PropertyName = "contactId")]
        public int Contact_ID { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string First_Name { get; set; }

        [JsonProperty(PropertyName = "nickname")]
        public string Nickname { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string Last_Name { get; set; }

        [JsonProperty(PropertyName = "birthDate")]
        public DateTime Date_Of_Birth { get; set; }

        [JsonProperty(PropertyName = "householdPosition")]
        public string Household_Position { get; set; }
    }
}