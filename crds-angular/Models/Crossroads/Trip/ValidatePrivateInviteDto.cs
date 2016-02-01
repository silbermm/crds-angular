using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class ValidatePrivateInviteDto
    {
        [JsonProperty(PropertyName = "valid")]
        public bool Valid { get; set; }
    }
}