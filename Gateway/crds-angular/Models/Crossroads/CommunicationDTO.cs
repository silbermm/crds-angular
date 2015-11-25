using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public class CommunicationDTO
    {
        [JsonProperty(PropertyName = "fromContact")]
        public int FromContactId { get; set; }

        [JsonProperty(PropertyName = "toContacts")]
        public List<int> ToContactIds { get; set; }

        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }

        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }
    }
}