using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class CheckScannerBatch
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "scan_date", ItemConverterType = typeof(JavaScriptDateTimeConverter))]
        public DateTime ScanDate { get; set; }

        [JsonProperty("status"), JsonConverter(typeof(StringEnumConverter))]
        public BatchStatus Status { get; set; }

        [JsonProperty("program_id")]
        public int? ProgramId { get; set; }

        #region Checks property and accessor
        [JsonIgnore]
        private readonly List<CheckScannerCheck> _checks = new List<CheckScannerCheck>();
        [JsonProperty("checks")]
        public List<CheckScannerCheck> Checks { get { return (_checks); } }
        #endregion
    }

    public enum BatchStatus
    {
        [EnumMember(Value = "not_exported")]
        NotExported,
        [EnumMember(Value = "exported")]
        Exported
    }
}