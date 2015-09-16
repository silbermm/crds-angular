using System;
using System.Collections.Generic;

namespace MinistryPlatform.Models
{
    public class Donation
    {
        public int donationId { get; set; }
        public int donorId { get; set; }
        public int donationAmt { get; set; }
        public DateTime donationDate { get; set; }
        public int paymentTypeId { get; set; }
        public string donationNotes { get; set; }
        public int donationStatus { get; set; }
        public DateTime donationStatusDate { get; set; }
        public int? batchId { get; set; }
        public string transactionCode { get; set; }
        public bool IncludeOnGivingHistory { get; set; }
        public bool IncludeOnPrintedStatement { get; set; }

        #region Distributions property
        private readonly List<DonationDistribution> _distributions = new List<DonationDistribution>();
        public List<DonationDistribution> Distributions { get { return (_distributions); } }
        #endregion

        public Donation()
        {
            IncludeOnGivingHistory = true;
            IncludeOnPrintedStatement = false;
        }
    }
}