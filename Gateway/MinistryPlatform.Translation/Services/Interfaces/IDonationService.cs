using System;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IDonationService
    {
        void UpdateDonationStatus(int donationId, int statusId, DateTime statusDate, string statusNote = null);
        void UpdateDonationStatus(string processorPaymentId, int statusId, DateTime statusDate, string statusNote = null);
    }
}