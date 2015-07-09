using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Services.Interfaces
{
    public interface IDonationService
    {
        void UpdateDonationStatus(int donationId, int statusId, DateTime? statusDate, string statusNote = null);
        void UpdateDonationStatus(string processorPaymentId, int statusId, DateTime? statusDate, string statusNote = null);
    }
}