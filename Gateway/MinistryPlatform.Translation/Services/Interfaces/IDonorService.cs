using System;
using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IDonorService
    {
        int CreateDonorRecord(int contactId, string stripeCustomerId, DateTime setupTime,
            int? statementFrequencyId = 1, // default to quarterly
            int? statementTypeId = 1, //default to individual
            int? statementMethodId = 2 // default to email/online
            );
        int CreateDonationAndDistributionRecord(int donationAmt, int donorId, string programId, string charge_id, DateTime setupDate);
        Donor GetDonorRecord(int contactId);
        Donor GetPossibleGuestDonorContact(string emailAddress);
        int UpdatePaymentProcessorCustomerId(int donorId, string paymentProcessorCustomerId);
    }
}
