using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.VolunteerApplication
{
    public class VolunteerApplicationDto
    {
        [JsonProperty(PropertyName = "contactId")]
        [Required]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "opportunityId")]
        [Required]
        public int OpportunityId { get; set; }

        [JsonProperty(PropertyName = "responseOpportunityId")]
        [Required]
        public int ResponseOpportunityId { get; set; }
    }
}