using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class TripGroupDto
    {
        [JsonProperty(PropertyName = "groupId")]
        public int GroupId { get; set; }

        [JsonProperty(PropertyName = "groupName")]
        public string GroupName { get; set; }
    }
}