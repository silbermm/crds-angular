using System;

namespace MinistryPlatform.Models
{
    public class DonationAndDistributionRecord
    {
        public DonationAndDistributionRecord(int donationAmt, int? feeAmt, int donorId, string programId, int? pledgeId, string chargeId, string pymtType, string processorId, DateTime setupDate, bool registeredDonor, bool recurringGift, int? recurringGiftId, string donorAcctId, string checkScannerBatchName = null, int? donationStatus = null)
        {
            DonationAmt = donationAmt;
            FeeAmt = feeAmt;
            DonorId = donorId;
            ProgramId = programId;
            PledgeId = pledgeId;
            ChargeId = chargeId;
            PymtType = pymtType;
            ProcessorId = processorId;
            SetupDate = setupDate;
            RegisteredDonor = registeredDonor;
            RecurringGift = recurringGift;
            RecurringGiftId = recurringGiftId;
            DonorAcctId = donorAcctId;
            CheckScannerBatchName = checkScannerBatchName;
            DonationStatus = donationStatus;
        }

        public int DonationAmt { get; private set; }

        public int? FeeAmt { get; private set; }

        public int DonorId { get; private set; }

        public string ProgramId { get; private set; }

        public int? PledgeId { get; private set; }

        public string ChargeId { get; private set; }

        public string PymtType { get; private set; }

        public string ProcessorId { get; private set; }

        public DateTime SetupDate { get; private set; }

        public bool RegisteredDonor { get; private set; }

        public bool RecurringGift { get; private set; }

        public int? RecurringGiftId { get; private set; }

        public string DonorAcctId { get; private set; }

        public string CheckScannerBatchName { get; private set; }

        public int? DonationStatus { get; private set; }
    }
}