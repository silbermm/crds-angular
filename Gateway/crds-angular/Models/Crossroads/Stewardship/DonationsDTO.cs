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
        /// A total of all the donation amounts included in Donations.
        /// </summary>
        [JsonProperty("donation_total_amount")]
        public int DonationTotalAmount { get { return (Donations.Select(d => d.Amount).Sum()); } }

        [JsonIgnore]
        public bool HasDonations { get { return (Donations.Count > 0); } }
    }
}