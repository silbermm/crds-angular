using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class EncryptValue
    {
        [JsonProperty("value")]
        public int value { get; set; }
    }
}