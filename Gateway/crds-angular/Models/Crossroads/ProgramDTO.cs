using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public class ProgramDTO
    {
        [JsonProperty(PropertyName = "programId")]
        public int ProgramId { get; set; }

        [JsonProperty(PropertyName = "communicationTemplateId", NullValueHandling = NullValueHandling.Ignore)]
        public int? CommunicationTemplateId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "programType")]
        public int ProgramType { get; set; }

        [JsonProperty(PropertyName = "allowRecurringGiving")]
        public bool AllowRecurringGiving { get; set; }
    }
}