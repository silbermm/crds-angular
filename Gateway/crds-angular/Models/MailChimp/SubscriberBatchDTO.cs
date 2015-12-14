using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.MailChimp
{
    public class SubscriberBatchDTO
    {
        [JsonProperty(PropertyName = "operations")]
        public List<SubscriberOperationDTO> Operations { get; set; }
    }
}