using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class TripCampaignDto
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "formId")]
        public int FormId { get; set; }
        [JsonProperty(PropertyName = "formName")]
        public string FormName { get; set; }

    }
}