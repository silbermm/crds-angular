using System;

namespace MinistryPlatform.Models
{
    public class DonationAndDistributionRecord
    {
        public DonationAndDistributionRecord(int donationAmt, int? feeAmt, int donorId, string programId, int? pledgeId, string chargeId, string pymtType, string processorId, DateTime setupDate, bool registeredDonor, bool anonymous, bool recurringGift, int? recurringGiftId, string donorAcctId, string checkScannerBatchName = null, int? donationStatus = null, string checkNumber = null)
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
            Anonymous = anonymous;
            RecurringGift = recurringGift;
            RecurringGiftId = recurringGiftId;
            DonorAcctId = donorAcctId;
            CheckScannerBatchName = checkScannerBatchName;
            DonationStatus = donationStatus;
            CheckNumber = checkNumber;
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

        public bool Anonymous { get; private set; }

        public string CheckNumber { get; private set; }

        protected bool Equals(DonationAndDistributionRecord other)
        {
            return Anonymous == other.Anonymous && string.Equals(ChargeId, other.ChargeId) && string.Equals(CheckNumber, other.CheckNumber) &&
                   string.Equals(CheckScannerBatchName, other.CheckScannerBatchName) && DonationAmt == other.DonationAmt && DonationStatus == other.DonationStatus &&
                   string.Equals(DonorAcctId, other.DonorAcctId) && DonorId == other.DonorId && FeeAmt == other.FeeAmt && PledgeId == other.PledgeId &&
                   string.Equals(ProcessorId, other.ProcessorId) && string.Equals(ProgramId, other.ProgramId) && string.Equals(PymtType, other.PymtType) &&
                   RecurringGift == other.RecurringGift && RecurringGiftId == other.RecurringGiftId && RegisteredDonor == other.RegisteredDonor && SetupDate.Equals(other.SetupDate);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((DonationAndDistributionRecord) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Anonymous.GetHashCode();
                hashCode = (hashCode*397) ^ (ChargeId != null ? ChargeId.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (CheckNumber != null ? CheckNumber.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (CheckScannerBatchName != null ? CheckScannerBatchName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ DonationAmt;
                hashCode = (hashCode*397) ^ DonationStatus.GetHashCode();
                hashCode = (hashCode*397) ^ (DonorAcctId != null ? DonorAcctId.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ DonorId;
                hashCode = (hashCode*397) ^ FeeAmt.GetHashCode();
                hashCode = (hashCode*397) ^ PledgeId.GetHashCode();
                hashCode = (hashCode*397) ^ (ProcessorId != null ? ProcessorId.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ProgramId != null ? ProgramId.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (PymtType != null ? PymtType.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ RecurringGift.GetHashCode();
                hashCode = (hashCode*397) ^ RecurringGiftId.GetHashCode();
                hashCode = (hashCode*397) ^ RegisteredDonor.GetHashCode();
                hashCode = (hashCode*397) ^ SetupDate.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(DonationAndDistributionRecord left, DonationAndDistributionRecord right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DonationAndDistributionRecord left, DonationAndDistributionRecord right)
        {
            return !Equals(left, right);
        }
    }
}