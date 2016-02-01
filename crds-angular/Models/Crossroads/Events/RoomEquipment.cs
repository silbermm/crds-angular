using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Events
{
    public class RoomEquipment
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }
    }
}