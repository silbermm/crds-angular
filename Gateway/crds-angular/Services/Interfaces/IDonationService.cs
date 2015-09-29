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
        List<DepositDTO> GetSelectedDonationBatches(int selectionId, string token);
        void ProcessDeclineEmail(string processorPaymentId);
        DepositDTO CreateDeposit(DepositDTO deposit);
        void CreatePaymentProcessorEventError(StripeEvent stripeEvent, StripeEventResponseDTO stripeEventResponse);
        DonationBatchDTO GetDonationBatchByProcessorTransferId(string processorTransferId);
        DonationBatchDTO GetDonationBatch(int batchId);
        DonationsDTO GetDonationsForAuthenticatedUser(string userToken, string donationYear = null, bool? softCredit = null);
        DonationYearsDTO GetDonationYearsForAuthenticatedUser(string userToken);
        DonationsDTO GetDonationsForDonor(int donorId, string donationYear = null, bool softCredit = false);
        DonationYearsDTO GetDonationYearsForDonor(int donorId);

            // ReSharper disable once InconsistentNaming
        List<GPExportDatumDTO> GetGPExport(int depositId, string token);
        // ReSharper disable once InconsistentNaming
        MemoryStream CreateGPExport(int selectionId, int depositId, string token);
        // ReSharper disable once InconsistentNaming
        string GPExportFileName(int depositId);
        // ReSharper disable once InconsistentNaming
        List<DepositDTO> GenerateGPExportFileNames(int selectionId, string token);

        void SendMessageToDonor(int donorId, int donationDistributionId, int fromContactId, string body, string tripName);
    }
}