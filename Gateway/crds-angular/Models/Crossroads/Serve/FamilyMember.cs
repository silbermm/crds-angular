using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Serve
{
    public class FamilyMember
    {
        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }
        
        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "participantId")]
        public int ParticipantId { get; set; }

        [JsonProperty(PropertyName = "preferredName")]
        public string PreferredName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "loggedInUser")]
        public bool LoggedInUser { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "relationshipId")]
        public int RelationshipId { get; set; }

        [JsonProperty(PropertyName = "highSchoolGraduationYear")]
        public int HighSchoolGraduationYear { get; set; }
    }
}