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

        public ContactDonor GetDonorForEmail(string emailAddress)
        {
            return (mpDonorService.GetPossibleGuestDonorContact(emailAddress));
        }

        public ContactDonor GetDonorForAuthenticatedUser(string authToken)
        {
            var contactId = authenticationService.GetContactId(authToken);
            return (mpDonorService.GetDonorRecord(contactId));
        }

        /// <summary>
        /// Creates or updates an MP Donor (and potentially creates a Contact) appropriately, based on the following logic:
        /// 1) If the given contactDonor is null, or if it does not represent an existing Contact,
        ///    create a Contact and Donor in MP, and create Customer in the payment processor system.  This
        ///    Contact and Donor will be considered a Guest Giver, unrelated to any registered User.
        ///    
        /// 2) If the given contactDonor is an existing contact, but does not have a payment processor customer,
        ///    create a Customer in the payment processor system, then either create a new Donor with the
        ///    payment processor Customer ID, or update the existing Donor (if any) with the id.
        ///    
        /// 3) If the given contactDonor is an existing contact, and an existing Donor with a Customer ID in the
        ///    payment processor system, simply return the given contactDonor.  This is a fallback, put in place
        ///    to take some of the decision logic out of the frontend on whether a new Donor needs to be created or not, 
        ///    whether a Customer needs to be created at the payment processor, etc.
        /// </summary>
        /// <param name="contactDonor">An existing ContactDonor, looked up from either GetDonorForEmail or GetDonorForAuthenticatedUser.  This may be null, indicating there is no existing contact or donor.</param>
        /// <param name="emailAddress">An email address to use when creating a Contact (#1 above).</param>
        /// <param name="paymentProcessorToken">The one-time-use token given by the payment processor.</param>
        /// <param name="setupDate">The date when the Donor is marked as setup - normally would be today's date.</param>
        /// <returns></returns>
        public ContactDonor CreateOrUpdateDonor(ContactDonor contactDonor, string emailAddress, string paymentProcessorToken, DateTime setupDate)
        {
            var contactDonorResponse = new ContactDonor();
            if (contactDonor == null || !contactDonor.ExistingContact)
            {
                contactDonorResponse.ContactId = mpContactService.CreateContactForGuestGiver(emailAddress, GUEST_GIVER_DISPLAY_NAME);
                contactDonorResponse.ProcessorId = paymentService.createCustomer(paymentProcessorToken);
                contactDonorResponse.DonorId = mpDonorService.CreateDonorRecord(contactDonorResponse.ContactId, contactDonorResponse.ProcessorId, setupDate, 
                    STATEMENT_FREQUENCY_NEVER, STATEMENT_TYPE_INDIVIDUAL, STATEMENT_METHOD_NONE);
                paymentService.updateCustomerDescription(contactDonorResponse.ProcessorId, contactDonorResponse.DonorId);
            } else if(!contactDonor.HasPaymentProcessorRecord) {
                contactDonorResponse.ContactId = contactDonor.ContactId;
                contactDonorResponse.ProcessorId = paymentService.createCustomer(paymentProcessorToken);
                if (contactDonor.ExistingDonor)
                {
                    contactDonorResponse.DonorId = mpDonorService.UpdatePaymentProcessorCustomerId(contactDonor.DonorId, contactDonorResponse.ProcessorId);
                }
                else
                {
                    contactDonorResponse.DonorId = mpDonorService.CreateDonorRecord(contactDonor.ContactId, contactDonorResponse.ProcessorId, setupDate,
                        STATEMENT_FREQUENCY_NEVER, STATEMENT_TYPE_INDIVIDUAL, STATEMENT_METHOD_NONE);
                }
                paymentService.updateCustomerDescription(contactDonorResponse.ProcessorId, contactDonorResponse.DonorId);
            }
            else
            {
                contactDonorResponse = contactDonor;
            }

            return (contactDonorResponse);
        }
    }
}