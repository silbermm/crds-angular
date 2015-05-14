using MinistryPlatform.Models;
using System;

namespace crds_angular.Services.Interfaces
{
    public interface IDonorService
    {
        ContactDonor GetDonorForEmail(string emailAddress);

        ContactDonor GetDonorForAuthenticatedUser(string authToken);

        ContactDonor CreateOrUpdateDonor(ContactDonor existingDonor, string emailAddress, string paymentProcessorToken, DateTime setupDate);
    }
}
