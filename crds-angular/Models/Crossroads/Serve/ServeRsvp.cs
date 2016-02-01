using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Serve
{
    public class ServeRsvp
    {
        [JsonProperty(PropertyName = "roleId")]
        public int RoleId { get; set; }

        [JsonProperty(PropertyName = "attending")]
        public bool Attending { get; set; }
    }
}