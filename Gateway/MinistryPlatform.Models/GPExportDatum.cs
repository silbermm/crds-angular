using System;

namespace MinistryPlatform.Models
{
    public class GPExportDatum
    {
        public int ProccessFeeProgramId { get; set; }
        public int ProgramId { get; set; }
        public string DocumentType { get; set; }
        public int DonationId { get; set; }
        public string BatchName { get; set; }
        public DateTime DonationDate { get; set; }
        public DateTime DepositDate { get; set; }
        public string CustomerId { get; set; }
        public string DonationAmount { get; set; }
        public string CheckbookId { get; set; }
        public string CashAccount { get; set; }
        public string ReceivableAccount { get; set; }
        public string DistributionAccount { get; set; }
        public string ScholarshipExpenseAccount { get; set; }
        public string Amount { get; set; }
        public int ScholarshipPaymentTypeId { get; set; }
        public int PaymentTypeId { get; set; }
    }
}