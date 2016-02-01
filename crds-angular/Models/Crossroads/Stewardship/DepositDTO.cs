﻿using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    // ReSharper disable once InconsistentNaming
    public class DepositDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("deposit_name")]
        public string DepositName { get; set; }

        [JsonProperty("deposit_total_amount")]
        public decimal DepositTotalAmount { get; set; }

        [JsonProperty("deposit_amount")]
        public decimal DepositAmount { get; set; }

        [JsonProperty("processor_fee_total")]
        public decimal ProcessorFeeTotal { get; set; }

        [JsonProperty("deposit_date_time")]
        public DateTime DepositDateTime { get; set; }

        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("batch_count")]
        public int BatchCount { get; set; }

        [JsonProperty("exported")]
        public bool Exported { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("processor_transfer_id")]
        public string ProcessorTransferId { get; set; }

        [JsonProperty("export_file_name")]
        public string ExportFileName { get; set; }
    }
}