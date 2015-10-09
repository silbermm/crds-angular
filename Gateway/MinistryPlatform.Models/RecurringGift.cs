using System;

namespace MinistryPlatform.Models
{
    public class RecurringGift
    {
        public int RecurringGiftId { get; set; }
        public int DonorID { get; set; }
        public string EmailAddress { get; set; }
        public string Frequency { get; set; }
        public string Recurrence { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Amount { get; set; }
        public string ProgramName { get; set; }
        public string CongregationName { get; set; }
        public int AccountTypeID { get; set; }
        public string AccountNumberLast4 { get; set; }
        public string InstitutionName { get; set; }
        public string SubscriptionID { get; set; }
    }
}