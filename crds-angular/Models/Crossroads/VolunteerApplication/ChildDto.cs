using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.VolunteerApplication
{
    public class ChildDto
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "birthdate")]
        public string Birthdate { get; set; }
    }
}