using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    // ReSharper disable once InconsistentNaming
    public class DonationYearsDTO
    {
        #region Available Donation Years property
        [JsonIgnore]
        private readonly List<string> _availableDonationYears = new List<string>();
        /// <summary>
        /// The list of years a donor has given, in no particular order.
        /// </summary>
        [JsonIgnore]
        public List<string> AvailableDonationYears { get { return (_availableDonationYears); } }
        /// <summary>
        /// The list of years a donor has given, in descending order
        /// </summary>
        [JsonProperty(PropertyName = "years", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> AvailableDonationYearsDescending { get { return (_availableDonationYears.OrderByDescending(i => i).ToList()); } }
        #endregion

        /// <summary>
        /// The year of the most recent donation.
        /// </summary>
        [JsonProperty(PropertyName = "most_recent_giving_year", NullValueHandling = NullValueHandling.Ignore)]
        public string MostRecentDonationYear { get { return (_availableDonationYears.Count > 0 ? AvailableDonationYearsDescending.First() : null); } }

        /// <summary>
        /// Returns true if there are any years, false if not.
        /// </summary>
        [JsonIgnore]
        public bool HasYears { get { return (_availableDonationYears.Count > 0); } }
    }
}