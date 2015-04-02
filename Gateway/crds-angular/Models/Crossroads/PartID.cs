using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public class PartID
    {
        [JsonProperty(PropertyName = "partId")]
        public List<int> partId { get; set; }
    }

}