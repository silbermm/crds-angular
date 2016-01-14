using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public class Congregation
    {
        [JsonProperty(PropertyName = "id")]
        public int CongregationId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "locationId")]
        public int LocationId { get; set; }
    }
}