using crds_angular.Models.Crossroads;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;


namespace crds_angular.Models.Json
{
    public class GroupDetailSerializer : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            var properties = jsonObject.Properties().ToList();
            return new GroupDetail()
            {
                groupID = Convert.ToInt32(jsonObject["groupID"]),
                groupFullInd = Convert.ToBoolean(jsonObject["groupFullInd"]),
                waitListInd = Convert.ToBoolean(jsonObject["waitListInd"]),
                waitListGroupId = Convert.ToInt32(jsonObject["waitListGroupId"])
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var groupDetail = value as GroupDetail;
            writer.WriteStartObject();
            writer.WritePropertyName("groupID");
            writer.WriteValue(groupDetail.groupID.ToString());
            writer.WritePropertyName("groupFullInd");
            writer.WriteValue(groupDetail.groupFullInd.ToString());
            writer.WritePropertyName("waitListInd");
            writer.WriteValue(groupDetail.waitListInd.ToString());
            writer.WritePropertyName("waitListGroupId");
            writer.WriteValue(groupDetail.waitListGroupId.ToString());
            writer.WriteEndObject();
        }
    }
}