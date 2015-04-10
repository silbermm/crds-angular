using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Serve
{
    public class ServeRole
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "capacity")]
        public Capacity Capacity { get; set; }

        [JsonProperty(PropertyName = "roleId")]
        public int RoleId { get; set; }
    }

    public class Capacity
    {
        [JsonProperty(PropertyName = "available")]
        public int Available { get; set; }

        [JsonProperty(PropertyName = "badgeType")]
        public string BadgeType { get; set; }

        [JsonProperty(PropertyName = "display")]
        public bool Display { get; set; }

        [JsonProperty(PropertyName = "maximum")]
        public int Maximum { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "minimum")]
        public int Minimum { get; set; }

        [JsonProperty(PropertyName = "taken")]
        public int Taken { get; set; }
        
    }
}