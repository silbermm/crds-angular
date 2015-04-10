using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Serve
{
    public class TeamMember
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "roles")]
        public List<ServeRole> Roles { get; set; }

        public ServeRsvp ServeRsvp {get; set; }

        public TeamMember()
        {
            this.Roles = new List<ServeRole>();
        }
    }

    public class ServeRsvp
    {
        public int RoleId { get; set; }
        public bool Attending { get; set; }
    }
}