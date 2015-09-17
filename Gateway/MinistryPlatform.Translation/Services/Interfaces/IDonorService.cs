using System;
using System.Collections.Generic;
using MinistryPlatform.Models;

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

        int CreateDonationAndDistributionRecord(int donationAmt, int? feeAmt, int donorId, string programId, string chargeId, string pymtType, string processorId, DateTime setupDate, bool registeredDonor, string checkScannerBatchName = null);
        ContactDonor GetContactDonor(int contactId);
        ContactDonor GetPossibleGuestContactDonor(string email);
        ContactDonor GetContactDonorForDonorAccount(string accountNumber, string routingNumber);
        int UpdatePaymentProcessorCustomerId(int donorId, string paymentProcessorCustomerId);
        void SetupConfirmationEmail(int programId, int donorId, int donationAmount, DateTime setupDate, string pymtType);
        ContactDonor GetEmailViaDonorId(int donorId);
        void SendEmail(int emailTemplate, int donorId, int donationAmount, string donationType, DateTime donationDate, string programName, string emailReason);
        ContactDetails GetContactDonorForCheckAccount(string encryptedKey);
        List<Donation> GetDonations(int donorId, string donationYear = null);
        List<Donation> GetDonations(IEnumerable<int> donorIds, string donationYear = null);
        List<Donation> GetSoftCreditDonations(int donorId);
    }
}
