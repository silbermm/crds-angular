using System;
using MPServices=MinistryPlatform.Translation.Services.Interfaces;
using crds_angular.Services.Interfaces;

namespace crds_angular.Services
{
    public class DonationService: IDonationService
    {
        private readonly MPServices.IDonationService _mpDonationService;

        public DonationService(MPServices.IDonationService mpDonationService)
        {
            _mpDonationService = mpDonationService;
        }

        public void UpdateDonationStatus(int donationId, int statusId, DateTime? statusDate, string statusNote = null)
        {
            _mpDonationService.UpdateDonationStatus(donationId, statusId, statusDate ?? DateTime.Now, statusNote);
        }

        public void UpdateDonationStatus(string processorPaymentId, int statusId, DateTime? statusDate, string statusNote = null)
        {
            _mpDonationService.UpdateDonationStatus(processorPaymentId, statusId, statusDate ?? DateTime.Now, statusNote);
        }
    }
}