using System;
using System.Net.Http;
using crds_angular.Models.Crossroads.Stewardship;

namespace crds_angular.Services.Interfaces
{
    public interface IDonationService
    {
        int UpdateDonationStatus(int donationId, int statusId, DateTime? statusDate, string statusNote = null);
        int UpdateDonationStatus(string processorPaymentId, int statusId, DateTime? statusDate, string statusNote = null);
        DonationDTO GetDonationByProcessorPaymentId(string processorPaymentId);
        DonationBatchDTO CreateDonationBatch(DonationBatchDTO batch);
        void ProcessDeclineEmail(string processorPaymentId);
        DepositDTO CreateDeposit(DepositDTO deposit);
        void CreatePaymentProcessorEventError(StripeEvent stripeEvent, StripeEventResponseDTO stripeEventResponse);
        DonationBatchDTO GetDonationBatchByProcessorTransferId(string processorTransferId);
        DonationBatchDTO GetDonationBatch(int batchId);
        ByteArrayContent CreateGPExport(int batchId, string token);
    }
}