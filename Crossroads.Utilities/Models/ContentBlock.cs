﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Crossroads.Utilities.Models
{
    public class ContentBlock
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("type"), JsonConverter(typeof(StringEnumConverter))]
        public ContentBlockType Type { get; set; }
        [JsonProperty("category"), JsonConverter(typeof(StringEnumConverter))]
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
        Corkboard,
        [EnumMember(Value = "trip application")]
        TripApplication
    }

    public class ContentBlocks
    {
        [JsonProperty("contentBlocks")]
        // RestSharp currently has an issue deserializing List not named the same as property,
        // so have to name this "contentBlocks" in order to deserialize properly
        //
        // ReSharper disable once InconsistentNaming
        public List<ContentBlock> contentBlocks { get; set; }
    }
}
