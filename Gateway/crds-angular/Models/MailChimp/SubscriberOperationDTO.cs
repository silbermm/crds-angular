using Newtonsoft.Json;

namespace crds_angular.Models.MailChimp
{
    public class SubscriberOperationDTO
    {
        [JsonProperty(PropertyName = "method")]
        public string Method { get; set; }
        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }
        [JsonProperty(PropertyName = "operation_id")]
        public string OperationId { get; set; }
        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }

    }
}