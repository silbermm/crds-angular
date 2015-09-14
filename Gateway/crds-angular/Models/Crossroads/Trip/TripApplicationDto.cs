using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class TripApplicationDto
    {
        [JsonProperty(PropertyName = "pageTwo")]
        public ApplicationPageTwo PageTwo { get; set; }

        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "pledgeCampaignId")]
        public int PledgeCampaignId { get; set; }

        public class ApplicationPageTwo
        {
            [JsonProperty(PropertyName = "allergies")]
            public TripApplicationField Allergies { get; set; }

            [JsonProperty(PropertyName = "conditions")]
            public TripApplicationField Conditions { get; set; }

            [JsonProperty(PropertyName = "guardianFirstName")]
            public TripApplicationField GuardianFirstName { get; set; }

            [JsonProperty(PropertyName = "guardianLastName")]
            public TripApplicationField GuardianLastName { get; set; }

            [JsonProperty(PropertyName = "referral")]
            public TripApplicationField Referral { get; set; }

            [JsonProperty(PropertyName = "scrubSize")]
            public TripApplicationField ScrubSize { get; set; }

            [JsonProperty(PropertyName = "spiritualLifeObedience")]
            public TripApplicationField SpiritualLifeObedience { get; set; }

            [JsonProperty(PropertyName = "spiritualLifeReceived")]
            public TripApplicationField SpiritualLifeReceived { get; set; }

            [JsonProperty(PropertyName = "spiritualLifeReplicating")]
            public TripApplicationField SpiritualLifeReplicating { get; set; }

            [JsonProperty(PropertyName = "spiritualLifeSearching")]
            public TripApplicationField SpiritualLifeSearching { get; set; }

            [JsonProperty(PropertyName = "tshirtSize")]
            public TripApplicationField TshirtSize { get; set; }

            [JsonProperty(PropertyName = "vegetarian")]
            public TripApplicationField Vegetarian { get; set; }

            [JsonProperty(PropertyName = "why")]
            public TripApplicationField Why { get; set; }
        }

        public class TripApplicationField
        {
            [JsonProperty(PropertyName = "formFieldId")]
            public int FormFieldId { get; set; }

            [JsonProperty(PropertyName = "attributeId")]
            public int AttributeId { get; set; }

            [JsonProperty(PropertyName = "value")]
            public string Value { get; set; }
        }
    }
}