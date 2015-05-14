using MinistryPlatform.Models;
using System;

namespace crds_angular.Services.Interfaces
{
    public interface IDonorService
    {
        Donor GetDonorForEmail(string emailAddress);

        Donor GetDonorForAuthenticatedUser(string authToken);

        Donor CreateDonor(Donor existingDonor, string emailAddress, string paymentProcessorToken, DateTime setupDate);
    }
}
