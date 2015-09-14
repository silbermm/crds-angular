using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class TripApplicationDto
    {
        [JsonProperty(PropertyName = "pageTwo")]
        public ApplicationPageTwo PageTwo { get; set; }

        public int ContactId { get; set; }
        public int PledgeCampaignId { get; set; }

        public class ApplicationPageTwo
        {
            [JsonProperty(PropertyName = "allergies")]
            public string Allergies { get; set; }

            [JsonProperty(PropertyName = "conditions")]
            public string Conditions { get; set; }

            [JsonProperty(PropertyName = "guardianFirstName")]
            public string GuardianFirstName { get; set; }

            [JsonProperty(PropertyName = "guardianLastName")]
            public string GuardianLastName { get; set; }

            [JsonProperty(PropertyName = "referral")]
            public string Referral { get; set; }

            [JsonProperty(PropertyName = "scrubSize")]
            public string ScrubSize { get; set; }

            [JsonProperty(PropertyName = "spiritualLifeObedience")]
            public string SpiritualLifeObedience { get; set; }

            [JsonProperty(PropertyName = "spiritualLifeReceived")]
            public string SpiritualLifeReceived { get; set; }

            [JsonProperty(PropertyName = "spiritualLifeReplicating")]
            public string SpiritualLifeReplicating { get; set; }

            [JsonProperty(PropertyName = "spiritualLifeSearching")]
            public string SpiritualLifeSearching { get; set; }

            [JsonProperty(PropertyName = "tshirtSize")]
            public string TshirtSize { get; set; }

            [JsonProperty(PropertyName = "vegetarian")]
            public string Vegetarian { get; set; }

            [JsonProperty(PropertyName = "why")]
            public string Why { get; set; }
        }
    }
}