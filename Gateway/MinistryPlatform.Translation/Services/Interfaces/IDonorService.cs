using System;
using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IDonorService
    {
        int CreateDonorRecord(int contactId, string stripeCustomerId, DateTime setupDate);
        int CreateDonationAndDistributionRecord(int donationAmt, int donorId, string programId, string charge_id, DateTime setupDate);
        Donor GetDonorRecord(int contactId);
    }
}
