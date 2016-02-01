using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Opportunity
{
    public class OpportunityGroupParticipant
    {
        [JsonProperty(PropertyName = "participantId")]
        public int ParticipantId { get; set; }

        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "nickname")]
        public string NickName { get; set; }

        [JsonProperty(PropertyName = "lastname")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "groupRoleId")]
        public int GroupRoleId { get; set; }

        [JsonProperty(PropertyName = "groupRoleTitle")]
        public string GroupRoleTitle { get; set; }
    }
}