using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Services
{
    public class DonorService : crds_angular.Services.Interfaces.IDonorService
    {
        private IDonorService mpDonorService;
        private IContactService mpContactService;
        private crds_angular.Services.Interfaces.IPaymentService paymentService;

        private const string GUEST_GIVER_DISPLAY_NAME = "Guest Giver";

        public DonorService(IDonorService mpDonorService, IContactService mpContactService, crds_angular.Services.Interfaces.IPaymentService paymentService)
        {
            this.mpDonorService = mpDonorService;
            this.mpContactService = mpContactService;
            this.paymentService = paymentService;
        }

        public Donor getDonorForEmail(string emailAddress)
        {
            return (mpDonorService.GetPossibleGuestDonorContact(emailAddress));
        }

        public void updateDonor(int donorId, string paymentProcessorId)
        {
            mpDonorService.UpdatePaymentProcessorCustomerId(donorId, paymentProcessorId);
        }

        public int createDonor(Donor existingDonor, string emailAddress, string paymentProcessorToken, DateTime setupDate)
        {
            int donorId;
            if (existingDonor == null)
            {
                var contactId = mpContactService.CreateContactForGuestGiver(emailAddress, GUEST_GIVER_DISPLAY_NAME);
                var paymentProcessorCustomerId = paymentService.createCustomer(paymentProcessorToken);
                donorId = mpDonorService.CreateDonorRecord(contactId, paymentProcessorCustomerId, setupDate);
            } else if(String.IsNullOrWhiteSpace(existingDonor.StripeCustomerId)) {
                var paymentProcessorCustomerId = paymentService.createCustomer(paymentProcessorToken);
                if (existingDonor.DonorId > 0)
                {
                    donorId = mpDonorService.UpdatePaymentProcessorCustomerId(existingDonor.DonorId, paymentProcessorCustomerId);
                }
                else
                {
                    donorId = mpDonorService.CreateDonorRecord(existingDonor.ContactId, paymentProcessorCustomerId, setupDate);
                }
            } else {
                donorId = existingDonor.DonorId;
            }

            return (donorId);
        }
    }
}