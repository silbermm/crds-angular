using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Events
{
    public class Room
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "buildingId")]
        public int BuildingId { get; set; }

        [JsonProperty(PropertyName = "locationId")]
        public int LocationId { get; set; }
    }
}