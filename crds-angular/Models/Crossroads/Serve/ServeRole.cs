using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Serve
{
    public class ServeRole
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "room")]
        public string Room { get; set; }

        [JsonProperty(PropertyName = "roleId")]
        public int RoleId { get; set; }

        [JsonProperty(PropertyName = "maximum")]
        public int? Maximum { get; set; }

        [JsonProperty(PropertyName = "minimum")]
        public int? Minimum { get; set; }

        [JsonProperty(PropertyName = "shiftEndTime")]
        public string ShiftEndTime { get; set; }

        [JsonProperty(PropertyName = "shiftStartTime")]
        public string ShiftStartTime { get; set; }
    }
}