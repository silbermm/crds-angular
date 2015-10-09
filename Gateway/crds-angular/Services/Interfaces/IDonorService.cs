using MinistryPlatform.Models;
using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads.Stewardship;
using MinistryPlatform.Models.DTO;

namespace crds_angular.Services.Interfaces
{
    public interface IDonorService
    {
        ContactDonor GetContactDonorForEmail(string emailAddress);

        ContactDonor GetContactDonorForAuthenticatedUser(string authToken);

        ContactDonor GetContactDonorForDonorAccount(string accountNumber, string routingNumber);

        ContactDonor GetContactDonorForDonorId(int donorId);
            
        ContactDonor GetContactDonorForCheckAccount(string encryptedKey);

        ContactDonor CreateOrUpdateContactDonor(ContactDonor existingDonor,  string encryptedKey, string emailAddress, string paymentProcessorToken, DateTime setupDate);

        string DecryptValues(string value);

        int CreateRecurringGift(RecurringGiftDto recurringGiftDto, ContactDonor contact);

        CreateDonationDistDto GetRecurringGiftForSubscription(string subscriptionId);

        List<RecurringGiftDto> GetRecurringGiftsForAuthenticatedUser(string userToken);
    }
}
