using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public class DonorStatementDTO
    {
        [JsonProperty(PropertyName = "donorId")]
        public int DonorId { get; set; }
        [JsonProperty(PropertyName = "paperless")]
        public bool Paperless { get; set; }      

    }
}