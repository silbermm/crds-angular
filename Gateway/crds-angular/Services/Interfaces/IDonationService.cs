using System;
using System.Collections.Generic;
using System.IO;
using crds_angular.Models.Crossroads.Stewardship;

namespace crds_angular.Services.Interfaces
{
    public interface IDonationService
    {
        int UpdateDonationStatus(int donationId, int statusId, DateTime? statusDate, string statusNote = null);
        int UpdateDonationStatus(string processorPaymentId, int statusId, DateTime? statusDate, string statusNote = null);
        DonationDTO GetDonationByProcessorPaymentId(string processorPaymentId);
        DonationBatchDTO CreateDonationBatch(DonationBatchDTO batch);
        DonationBatchDTO GetDonationBatchByDepositId(int depositId);
        List<DonationBatchDTO> GetSelectedDonationBatches(int selectionId, string token);
        void ProcessDeclineEmail(string processorPaymentId);
        DepositDTO CreateDeposit(DepositDTO deposit);
        void CreatePaymentProcessorEventError(StripeEvent stripeEvent, StripeEventResponseDTO stripeEventResponse);
        DonationBatchDTO GetDonationBatchByProcessorTransferId(string processorTransferId);
        DonationBatchDTO GetDonationBatch(int batchId);
        MemoryStream CreateGPExport(int depositId, string token);
        void GPExportFileName(DonationBatchDTO depositId);
        void UpdateDepositToExported(int depositId);
        List<DonationBatchDTO> GenerateGPExportFileNames(int selectionId, string token);
    }
}