using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Serve
{
    public class ServeRole
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "capacity")]
        public int Capacity { get; set; }

        [JsonProperty(PropertyName = "slotsTaken")]
        public int SlotsTaken { get; set; }

        [JsonProperty(PropertyName = "roleId")]
        public int RoleId { get; set; }
    }
}