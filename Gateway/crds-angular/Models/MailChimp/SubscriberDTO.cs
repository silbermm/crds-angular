using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.MailChimp
{
    public class SubscriberDTO
    {
        [JsonIgnore]
        public bool Subscribed { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string Status
        {
            get { return Subscribed ? "subscribed" : "unsubscribed"; }
        }
        [JsonProperty(PropertyName = "email_address")]
        public string EmailAddress { get; set; }
        [JsonProperty(PropertyName = "merge_fields")]
        public Dictionary<string, string> MergeFields { get; set; }
    }
}