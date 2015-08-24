using System;

namespace MinistryPlatform.Models
{
    public class GPExportDatum
    {
        public static readonly string[] Headers =
        {
            "Document Type", "Document Number", "Document Description", "Batch ID", "Contribution Date", "Settlement Date", "Customer ID",
            "Contribution Amount", "Checkbook ID", "Cash Account", "Receivable Account", "Distribution Account", "Distribution Amount",
            "Distribution Reference"
        };

        public string DocumentType { get; set; }
        public int DocumentNumber { get; set; }
        public string DocumentDescription { get; set; }
        public string BatchId { get; set; }
        public DateTime ContributionDate { get; set; }
        public DateTime SettlementDate { get; set; }
        public string CustomerId { get; set; }
        public string ContributionAmount { get; set; }
        public string CheckbookId { get; set; }
        public string CashAccount { get; set; }
        public string ReceivableAccount { get; set; }
        public string DistributionAccount { get; set; }
        public string DistributionAmount { get; set; }
        public string DistributionReference { get; set; }
    }
}