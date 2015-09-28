using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class DecryptValue
    {
        [JsonProperty("value")]
        public int value { get; set; }
    }
}