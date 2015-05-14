using MinistryPlatform.Models;
using System;

namespace crds_angular.Services.Interfaces
{
    public interface IDonorService
    {
        ContactDonor GetContactDonorForEmail(string emailAddress);

        ContactDonor GetContactDonorForAuthenticatedUser(string authToken);

        ContactDonor CreateOrUpdateContactDonor(ContactDonor existingDonor, string emailAddress, string paymentProcessorToken, DateTime setupDate);
    }
}
