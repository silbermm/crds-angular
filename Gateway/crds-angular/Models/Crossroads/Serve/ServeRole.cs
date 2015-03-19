using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Serve
{
    public class ServeRole
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}