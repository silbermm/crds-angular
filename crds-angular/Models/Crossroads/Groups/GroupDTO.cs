using System.Collections.Generic;
using crds_angular.Models.Crossroads.Events;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Groups
{
    
    public class GroupDTO
    {
        [JsonProperty(PropertyName = "groupName")]
        public string GroupName { get; set; }

        [JsonProperty(PropertyName = "groupId")]
        public int GroupId { get; set; }

        [JsonProperty(PropertyName = "groupFullInd")]
        public bool GroupFullInd { get; set; }

        [JsonProperty(PropertyName = "waitListInd")]
        public bool WaitListInd { get; set; }

        [JsonProperty(PropertyName = "waitListGroupId")]
        public int WaitListGroupId { get; set; }

        [JsonProperty(PropertyName = "childCareInd")]
        public bool ChildCareAvailable { get; set; }

        [JsonProperty(PropertyName = "minAge")]
        public int OnlineRsvpMinimumAge { get; set; }

        public List<SignUpFamilyMembers> SignUpFamilyMembers { get; set; }

        [JsonProperty(PropertyName = "events")]
        public List<Event> Events { get; set; } 
        
    }

   
}