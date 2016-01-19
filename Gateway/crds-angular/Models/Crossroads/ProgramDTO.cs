using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public class ProgramDTO
    {
        public int ProgramId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? CommunicationTemplateId { get; set; }

        public string Name { get; set; }

        public int ProgramType { get; set; }

        public bool AllowRecurringGiving { get; set; }
    }
}