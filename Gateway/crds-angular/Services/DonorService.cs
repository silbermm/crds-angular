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

        public const string GUEST_GIVER_DISPLAY_NAME = "Guest Giver";

        public DonorService(IDonorService mpDonorService, IContactService mpContactService, crds_angular.Services.Interfaces.IPaymentService paymentService)
        {
            this.mpDonorService = mpDonorService;
            this.mpContactService = mpContactService;
            this.paymentService = paymentService;
        }

        public Donor GetDonorForEmail(string emailAddress)
        {
            return (mpDonorService.GetPossibleGuestDonorContact(emailAddress));
        }

        public Donor CreateDonor(Donor existingDonor, string emailAddress, string paymentProcessorToken, DateTime setupDate)
        {
            var donor = new Donor();
            if (existingDonor == null)
            {
                donor.ContactId = mpContactService.CreateContactForGuestGiver(emailAddress, GUEST_GIVER_DISPLAY_NAME);
                donor.StripeCustomerId = paymentService.createCustomer(paymentProcessorToken);
                donor.DonorId = mpDonorService.CreateDonorRecord(donor.ContactId, donor.StripeCustomerId, setupDate);
            } else if(String.IsNullOrWhiteSpace(existingDonor.StripeCustomerId)) {
                donor.ContactId = existingDonor.ContactId;
                donor.StripeCustomerId = paymentService.createCustomer(paymentProcessorToken);
                if (existingDonor.DonorId > 0)
                {
                    donor.DonorId = mpDonorService.UpdatePaymentProcessorCustomerId(existingDonor.DonorId, donor.StripeCustomerId);
                }
                else
                {
                    donor.DonorId = mpDonorService.CreateDonorRecord(existingDonor.ContactId, donor.StripeCustomerId, setupDate);
                }
            } else {
                donor.ContactId = existingDonor.ContactId;
                donor.DonorId = existingDonor.DonorId;
                donor.StripeCustomerId = existingDonor.StripeCustomerId;
            }

            return (donor);
        }
    }
}