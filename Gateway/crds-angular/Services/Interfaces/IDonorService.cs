using MinistryPlatform.Models;
using System;

namespace crds_angular.Services.Interfaces
{
    public interface IDonorService
    {
        Donor getDonorForEmail(string emailAddress);

        void updateDonor(int donorId, string paymentProcessorId);

        int createDonor(Donor existingDonor, string emailAddress, string paymentProcessorToken, DateTime setupDate);
    }
}
