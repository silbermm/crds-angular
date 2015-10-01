using System;
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

        int CreateDonationAndDistributionRecord(int donationAmt, int? feeAmt, int donorId, string programId, string chargeId, string pymtType, string processorId, DateTime setupDate, bool registeredDonor, bool recurringGift, string checkScannerBatchName = null);
        ContactDonor GetContactDonor(int contactId);
        ContactDonor GetPossibleGuestContactDonor(string email);
        ContactDonor GetContactDonorForDonorAccount(string accountNumber, string routingNumber);
        int UpdatePaymentProcessorCustomerId(int donorId, string paymentProcessorCustomerId);
        void SetupConfirmationEmail(int programId, int donorId, int donationAmount, DateTime setupDate, string pymtType);
        ContactDonor GetEmailViaDonorId(int donorId);
        void SendEmail(int emailTemplate, int donorId, int donationAmount, string donationType, DateTime donationDate, string programName, string emailReason);
        ContactDonor GetContactDonorForCheckAccount(string encryptedKey);
        string CreateHashedAccountAndRoutingNumber(string accountNumber, string routingNumber);
        string DecryptCheckValue(string value);
        void UpdateDonorAccount(string encryptedKey, string customerId, string sourceId);
        List<Donation> GetDonations(int donorId, string donationYear = null);
        List<Donation> GetDonations(IEnumerable<int> donorIds, string donationYear = null);
        List<Donation> GetSoftCreditDonations(IEnumerable<int> donorIds, string donationYear = null);
        List<Donation> GetDonationsForAuthenticatedUser(string userToken, bool? softCredit = null, string donationYear = null);
        CreateDonationDistDto GetRecurringGiftForSubscription(string subscription);
        int CreateDonorAccount(string institutionName, string routingNumber, string acctNumber, int donorId, string processorAcctId, string processorId);
        int CreateRecurringGiftRecord(int donorId, int donorAccountId, string planInterval, decimal planAmount, DateTime startDate, string program, string subscriptionId);
    }
}
