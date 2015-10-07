using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public class EmailCommunicationDTO
    {
        [JsonProperty(PropertyName = "fromContactId")]
        public int FromContactId { get; set; }

        [JsonProperty(PropertyName = "fromUserId", NullValueHandling = NullValueHandling.Ignore)]
        public int? FromUserId { get; set; }

        [JsonProperty(PropertyName = "toContactId")]
        public int ToContactId { get; set; }

        [JsonProperty(PropertyName = "templateId")]
        public int TemplateId { get; set; }

        [JsonProperty(PropertyName = "mergeData")]
        public Dictionary<string, object> MergeData { get; set; }
    }
}