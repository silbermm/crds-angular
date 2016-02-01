using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Donations.
    /// </summary>
    public class DonationsDTO
    {
        #region Donations List
        [JsonIgnore]
        private readonly List<DonationDTO> _donations = new List<DonationDTO>();
        /// <summary>
        /// A list of donations.
        /// </summary>
        [JsonProperty(PropertyName = "donations", NullValueHandling = NullValueHandling.Ignore)]
        public List<DonationDTO> Donations { get { return (_donations); } }
        #endregion
        /// <summary>
        /// A total of all the donation amounts included in Donations that appear on the giving history.
        /// </summary>
        [JsonProperty("donation_total_amount")]
        public int DonationTotalAmount { get { return (Donations.Select(d => d.IncludeOnGivingHistory && d.Status != DonationStatus.Declined ? d.Amount : 0).Sum()); } }

        /// <summary>
        /// A total of all the donation amounts included in Donations that appear on the printed statement.
        /// </summary>
        [JsonProperty("donation_statement_total_amount")]
        public int DonationStatementTotalAmount { get { return (Donations.Select(d => d.IncludeOnPrintedStatement && d.Status != DonationStatus.Declined ? d.Amount : 0).Sum()); } }

        [JsonIgnore]
        public bool HasDonations { get { return (Donations.Count > 0); } }

        [JsonProperty("beginning_donation_date")]
        public DateTime BeginningDonationDate { get; set; }

        [JsonProperty("ending_donation_date")]
        public DateTime EndingDonationDate { get; set; }
    }
}