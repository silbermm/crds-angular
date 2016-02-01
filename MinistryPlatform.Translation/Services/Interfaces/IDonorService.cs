﻿using System;
using System.Collections.Generic;
using MinistryPlatform.Models;
using MinistryPlatform.Models.DTO;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IDonorService
    {
        int CreateDonorRecord(int contactId, string processorId, DateTime setupTime,
            int? statementFrequencyId = 1, // default to quarterly
            int? statementTypeId = 1, //default to individual
            int? statementMethodId = 2, // default to email/online
            DonorAccount donorAccount = null
            );

        int CreateDonationAndDistributionRecord(DonationAndDistributionRecord donationAndDistribution);
        ContactDonor GetContactDonor(int contactId);
        ContactDonor GetPossibleGuestContactDonor(string email);
        ContactDonor GetContactDonorForDonorAccount(string accountNumber, string routingNumber);
        int UpdatePaymentProcessorCustomerId(int donorId, string paymentProcessorCustomerId);
        void SetupConfirmationEmail(int programId, int donorId, decimal donationAmount, DateTime setupDate, string pymtType);
        ContactDonor GetEmailViaDonorId(int donorId);
        void SendEmail(int emailTemplate, int donorId, decimal donationAmount, string donationType, DateTime donationDate, string programName, string emailReason, string frequency = null);
        ContactDonor GetContactDonorForCheckAccount(string encryptedKey);
        string CreateHashedAccountAndRoutingNumber(string accountNumber, string routingNumber);
        string DecryptCheckValue(string value);
        string UpdateDonorAccount(string encryptedKey, string customerId, string sourceId);
        List<Donation> GetDonations(int donorId, string donationYear = null);
        List<Donation> GetDonations(IEnumerable<int> donorIds, string donationYear = null);
        List<Donation> GetSoftCreditDonations(IEnumerable<int> donorIds, string donationYear = null);
        List<Donation> GetDonationsForAuthenticatedUser(string userToken, bool? softCredit = null, string donationYear = null);
        CreateDonationDistDto GetRecurringGiftForSubscription(string subscription);
        CreateDonationDistDto GetRecurringGiftById(string authorizedUserToken, int recurringGiftId);
        int CreateRecurringGiftRecord(string authorizedUserToken, int donorId, int donorAccountId, string planInterval, decimal planAmount, DateTime startDate, string program, string subscriptionId, int congregationId);
        void UpdateRecurringGiftDonorAccount(string authorizedUserToken, int recurringGiftId, int donorAccountId);
        void CancelRecurringGift(string authorizedUserToken, int recurringGiftId);
        void CancelRecurringGift(int recurringGiftId);
        int CreateDonorAccount(string institutionName, string routingNumber, string acctNumber, string encryptedAcct, int donorId, string processorAcctId, string processorId);
        void DeleteDonorAccount(string authorizedUserToken, int donorAccountId);
        List<RecurringGift> GetRecurringGiftsForAuthenticatedUser(string userToken);
        void ProcessRecurringGiftDecline(string subscription_id);
        void UpdateRecurringGiftFailureCount(int recurringGiftId, int failureCount);
        void UpdateRecurringGift(int pageView, string token, int recurringGiftId, Dictionary<string, object> recurringGiftValues);
        int GetDonorAccountPymtType(int donorAccountId);

        DonorStatement GetDonorStatement(string token);
        void UpdateDonorStatement(string token, DonorStatement statement);
    }
}
