using System;
using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IDonorService
    {
        int CreateDonorRecord(int contactId, string stripeCustomerId);
        int CreateDonationRecord(int contactId, int donationAmt, DateTime donationDate, int paymentType);
        int CreateDonationDistributionRecord(int donationId, int donationAmt, string program);
    }
}
