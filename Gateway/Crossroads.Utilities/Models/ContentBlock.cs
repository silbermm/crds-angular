using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Crossroads.Utilities.Models
{
    public class ContentBlock
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("type")]
        public ContentBlockType Type { get; set; }
        [JsonProperty("category")]
        public ContentBlockCategory Category { get; set; }
    }

    public enum ContentBlockType
    {
        [EnumMember(Value = "success")]
        Success,
        [EnumMember(Value = "info")]
        Info,
        [EnumMember(Value = "warning")]
        Warning,
        [EnumMember(Value = "error")]
        Error
    }

    public enum ContentBlockCategory
    {
        [EnumMember(Value = "common")]
        Common,
        [EnumMember(Value = "main")]
        Main,
        [EnumMember(Value = "corkboard")]
        Corkboard
    }
}