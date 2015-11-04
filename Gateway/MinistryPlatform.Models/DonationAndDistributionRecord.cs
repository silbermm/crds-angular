using System;

namespace MinistryPlatform.Models
{
    public class DonationAndDistributionRecord
    {
        public int DonationAmt { get; set; }

        public int? FeeAmt { get; set; }

        public int DonorId { get; set; }

        public string ProgramId { get; set; }

        public int? PledgeId { get; set; }

        public string ChargeId { get; set; }

        public string PymtType { get; set; }

        public string ProcessorId { get; set; }

        public DateTime SetupDate { get; set; }

        public bool RegisteredDonor { get; set; }

        public bool RecurringGift { get; set; }

        public int? RecurringGiftId { get; set; }

        public string DonorAcctId { get; set; }

        public string CheckScannerBatchName { get; set; }

        public int? DonationStatus { get; set; }

        public bool Anonymous { get; set; }

        public string CheckNumber { get; set; }
    }
}