using System;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IDonationService
    {
        int UpdateDonationStatus(int donationId, int statusId, DateTime statusDate, string statusNote = null);
        int UpdateDonationStatus(string processorPaymentId, int statusId, DateTime statusDate, string statusNote = null);
        int CreateDonationBatch(string batchName, DateTime setupDateTime, decimal batchTotalAmount, int itemCount,
            int batchEntryType, int? depositId, DateTime finalizedDateTime, string processorTransferId);
        void AddDonationToBatch(int batchId, int donationId);
        void ProcessDeclineEmail(string processorPaymentId);
        int CreateDeposit(string depositName, decimal depositTotalAmount, DateTime depositDateTime, string accountNumber, int batchCount, bool exported, string notes, string processorTransferId);
        void CreatePaymentProcessorEventError(DateTime? eventDateTime, string eventId, string eventType,
            string eventMessage, string responseMessage);
    }
}