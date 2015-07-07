using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using crds_angular.Models.Json;

namespace crds_angular.Models.Crossroads
{
    public class StripeEvent : StripeObject
    {
        [JsonProperty("type")]
        [Required(ErrorMessage = "Event type is required")]
        public string Type { get; set; }

        [JsonProperty("created")]
        [JsonConverter(typeof(StripeDateTimeConverter))]
        public DateTime? Created { get; set; }

        [JsonProperty("data")]
        [Required(ErrorMessage = "Event data is required")]
        public StripeEventData Data { get; set; }
    }

    public class StripeEventData
    {
        [JsonProperty("object")]
        [Required(ErrorMessage = "Event data object is required")]
        public dynamic Object { get; set; }
    }
}