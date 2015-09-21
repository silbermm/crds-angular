using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class TripCampaignDto
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "formId")]
        public int FormId { get; set; }

        [JsonProperty(PropertyName = "nickname")]
        public string Nickname { get; set; }

        [JsonProperty(PropertyName = "ageLimit")]
        public int YoungestAgeAllowed { get; set; }

        [JsonProperty(PropertyName = "registrationStart")]
        public DateTime RegistrationStart { get; set; }

        [JsonProperty(PropertyName = "registrationEnd")]
        public DateTime RegistrationEnd { get; set; }

        [JsonProperty(PropertyName = "ageExceptions")]
        public List<int> AgeExceptions { get; set; }
    }
}