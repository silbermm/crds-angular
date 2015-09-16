using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class StripeSource : StripeObject
    {
        [JsonProperty(PropertyName = "last4", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountNumberLast4 { get; set; }

        [JsonProperty(PropertyName = "brand", NullValueHandling = NullValueHandling.Ignore), JsonConverter(typeof(StringEnumConverter))]
        public CardBrand? Brand { get; set; }
    }

    public enum CardBrand
    {
        [EnumMember(Value = "Visa")]
        Visa,
        [EnumMember(Value = "American Express")]
        AmericanExpress,
        [EnumMember(Value = "MasterCard")]
        MasterCard,
        [EnumMember(Value = "Discover")]
        Discover,
        [EnumMember(Value = "JCB")]
        JCB,
        [EnumMember(Value = "Diners Club")]
        DinersClub,
        [EnumMember(Value = "Unknown")]
        Unknown
    }
}