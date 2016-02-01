using System;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace crds_angular.Models.Json
{
    public class StripeDateTimeConverter : DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(value == null
                ? "0"
                : ((DateTime) value).ConvertDateTimeToEpoch().ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) return null;

            if (reader.TokenType == JsonToken.Integer)
                return StripeEpochTime.ConvertEpochToDateTime((long)reader.Value);

            return DateTime.Parse(reader.Value.ToString());
        }
    }
}