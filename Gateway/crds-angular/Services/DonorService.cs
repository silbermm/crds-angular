using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using System;

namespace crds_angular.Services
{
    public class DonorService : crds_angular.Services.Interfaces.IDonorService
    {
        private IDonorService mpDonorService;
        private IContactService mpContactService;
        private crds_angular.Services.Interfaces.IPaymentService paymentService;
        private IConfigurationWrapper configurationWrapper;
        private IAuthenticationService authenticationService;

        private readonly string GUEST_GIVER_DISPLAY_NAME;

        private readonly int STATEMENT_FREQUENCY_NEVER;
        private readonly int STATEMENT_TYPE_INDIVIDUAL;
        private readonly int STATEMENT_METHOD_NONE;

        public DonorService(IDonorService mpDonorService, IContactService mpContactService,
            crds_angular.Services.Interfaces.IPaymentService paymentService, IConfigurationWrapper configurationWrapper,
            IAuthenticationService authenticationService)
        {
            this.mpDonorService = mpDonorService;
            this.mpContactService = mpContactService;
            this.paymentService = paymentService;
            this.configurationWrapper = configurationWrapper;
            this.authenticationService = authenticationService;

            GUEST_GIVER_DISPLAY_NAME = configurationWrapper.GetConfigValue("GuestGiverContactDisplayName");
            STATEMENT_FREQUENCY_NEVER = configurationWrapper.GetConfigIntValue("DonorStatementFrequencyNever");
            STATEMENT_TYPE_INDIVIDUAL = configurationWrapper.GetConfigIntValue("DonorStatementTypeIndividual");
            STATEMENT_METHOD_NONE = configurationWrapper.GetConfigIntValue("DonorStatementMethodNone");
        }

        public Donor GetDonorForEmail(string emailAddress)
        {
            return (mpDonorService.GetPossibleGuestDonorContact(emailAddress));
        }

        public Donor GetDonorForAuthenticatedUser(string authToken)
        {
            var contactId = authenticationService.GetContactId(authToken);
            return (mpDonorService.GetDonorRecord(contactId));
        }

        public Donor CreateDonor(Donor existingDonor, string emailAddress, string paymentProcessorToken, DateTime setupDate)
        {
            var donor = new Donor();
            if (existingDonor == null)
            {
                donor.ContactId = mpContactService.CreateContactForGuestGiver(emailAddress, GUEST_GIVER_DISPLAY_NAME);
                donor.StripeCustomerId = paymentService.createCustomer(paymentProcessorToken);
                donor.DonorId = mpDonorService.CreateDonorRecord(donor.ContactId, donor.StripeCustomerId, setupDate, 
                    STATEMENT_FREQUENCY_NEVER, STATEMENT_TYPE_INDIVIDUAL, STATEMENT_METHOD_NONE);
                paymentService.updateCustomerDescription(donor.StripeCustomerId, donor.DonorId);
            } else if(String.IsNullOrWhiteSpace(existingDonor.StripeCustomerId)) {
                donor.ContactId = existingDonor.ContactId;
                donor.StripeCustomerId = paymentService.createCustomer(paymentProcessorToken);
                if (existingDonor.DonorId > 0)
                {
                    donor.DonorId = mpDonorService.UpdatePaymentProcessorCustomerId(existingDonor.DonorId, donor.StripeCustomerId);
                }
                else
                {
                    donor.DonorId = mpDonorService.CreateDonorRecord(existingDonor.ContactId, donor.StripeCustomerId, setupDate,
                        STATEMENT_FREQUENCY_NEVER, STATEMENT_TYPE_INDIVIDUAL, STATEMENT_METHOD_NONE);
                }
                paymentService.updateCustomerDescription(donor.StripeCustomerId, donor.DonorId);
            }
            else
            {
                donor.ContactId = existingDonor.ContactId;
                donor.DonorId = existingDonor.DonorId;
                donor.StripeCustomerId = existingDonor.StripeCustomerId;
            }

            return (donor);
        }
    }
}