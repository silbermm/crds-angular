using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public class SignUpFamilyMembers

    {
        [JsonProperty(PropertyName = "nickName")]
        public string PreferredName { get; set; }

        [JsonProperty(PropertyName = "emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "userInGroup")]
        public bool UserInGroup { get; set; }

        [JsonProperty(PropertyName = "participantId")]
        public int ParticpantId { get; set; }

        [JsonProperty(PropertyName = "childCareNeeded")]
        public bool ChildCareNeeded { get; set; }
    }
}