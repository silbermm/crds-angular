using System.Collections.Generic;
using MinistryPlatform.Models;
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

        [JsonProperty(PropertyName = "index")]
        public long Index { get; set; }

        [JsonProperty(PropertyName = "roles")]
        public List<ServeRole> Roles { get; set; }

        [JsonProperty(PropertyName = "serveRsvp")]
        public ServeRsvp ServeRsvp { get; set; }

        [JsonIgnore]
        public Participant Participant { get; set; }

        [JsonIgnore]
        public List<MinistryPlatform.Models.Response> Responses { get; set; }

        public TeamMember()
        {
            this.Roles = new List<ServeRole>();
            this.Responses = new List<MinistryPlatform.Models.Response>();
        }
    }
}