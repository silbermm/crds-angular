using System;
using Newtonsoft.Json;

namespace MinistryPlatform.Models
{
    public class GPExportDatum
    {
        [JsonProperty(PropertyName = "Document Type", Order = 1)]
        public string DocumentType { get; set; }

        [JsonProperty(PropertyName = "Document Number", Order = 2)]
        public int DocumentNumber { get; set; }

        [JsonProperty(PropertyName = "Document Description", Order = 3)]
        public string DocumentDescription { get; set; }

        [JsonProperty(PropertyName = "Batch ID", Order = 4)]
        public string BatchId { get; set; }

        [JsonProperty(PropertyName = "Contribution Date", Order = 5)]
        public DateTime ContributionDate { get; set; }

        [JsonProperty(PropertyName = "Settlement Date", Order = 6)]
        public DateTime SettlementDate { get; set; }

        [JsonProperty(PropertyName = "Customer ID", Order = 7)]
        public string CustomerId { get; set; }

        [JsonProperty(PropertyName = "Contribution Amount", Order = 8)]
        public string ContributionAmount { get; set; }

        [JsonProperty(PropertyName = "Checkbook ID", Order = 9)]
        public string CheckbookId { get; set; }

        [JsonProperty(PropertyName = "Cash Account", Order = 10)]
        public string CashAccount { get; set; }

        [JsonProperty(PropertyName = "Receivable Account", Order = 11)]
        public string ReceivableAccount { get; set; }

        [JsonProperty(PropertyName = "Distribution Account", Order = 12)]
        public string DistributionAccount { get; set; }

        [JsonProperty(PropertyName = "Distribution Amount", Order = 13)]
        public string DistributionAmount { get; set; }

        [JsonProperty(PropertyName = "Distribution Reference", Order = 14)]
        public string DistributionReference { get; set; }
    }
}
 