using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Events
{
    public class RoomLayout
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "layoutName")]
        public string LayoutName { get; set; }
    }
}