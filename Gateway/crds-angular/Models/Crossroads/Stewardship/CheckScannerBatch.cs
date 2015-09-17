using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
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

        [JsonProperty(PropertyName = "scanDate"), JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime ScanDate { get; set; }

        [JsonProperty("status"), JsonConverter(typeof(StringEnumConverter))]
        public BatchStatus Status { get; set; }

        [JsonProperty("programId", NullValueHandling = NullValueHandling.Ignore)]
        public int? ProgramId { get; set; }

        #region Checks property and accessor
        [JsonIgnore]
        private readonly List<CheckScannerCheck> _checks = new List<CheckScannerCheck>();
        [JsonProperty("checks")]
        public List<CheckScannerCheck> Checks { get { return (_checks); } }
        #endregion

        #region Errored Checks properties and accessors
        [JsonIgnore]
        private readonly List<CheckScannerCheck> _errorChecks = new List<CheckScannerCheck>();
        [JsonProperty("errorChecks")]
        public List<CheckScannerCheck> ErrorChecks { get { return (_errorChecks); } }
        [JsonIgnore]
        private List<String> ChecksEmailErrors { get { return ErrorChecks.Select(check => check.EmailError).ToList(); } }
        [JsonIgnore]
        public string EmailMsg
        {
            get
            {
                if (!ErrorChecks.Any())
                {
                    return SuccessfulEmailMsg();
                }

                return ErrorEmailMsg();
            }
        }

        private string SuccessfulEmailMsg()
        {
            return string.Format("All {0} Checks Processed Successfully.", Checks.Count);
        }

        private string ErrorEmailMsg()
        {
            var emailErrors = new StringBuilder();
            emailErrors.AppendFormat("{0} Checks Processed Successfully.\n", Checks.Count);
            emailErrors.AppendFormat("{0} Checks Failed With The Following Error Details:\n\n", ErrorChecks.Count);

            foreach (var emailError in ChecksEmailErrors)
            {
                emailErrors.Append(emailError);
            }

            return emailErrors.ToString();
        }
        #endregion

        [JsonProperty("contactId", NullValueHandling = NullValueHandling.Ignore)]
        public int? MinistryPlatformContactId { get; set; }

        [JsonProperty("userId", NullValueHandling = NullValueHandling.Ignore)]
        public int? MinistryPlatformUserId { get; set; }
    }

    public enum BatchStatus
    {
        [EnumMember(Value = "notExported")]
        NotExported,
        [EnumMember(Value = "exported")]
        Exported
    }
}