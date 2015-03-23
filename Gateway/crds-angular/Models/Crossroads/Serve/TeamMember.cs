using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Serve
{
    public class TeamMember
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "roles")]
        public List<ServeRole> Roles { get; set; }

        public TeamMember()
        {
            this.Roles = new List<ServeRole>();
        }
    }
}