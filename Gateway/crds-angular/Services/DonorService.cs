using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using System;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads.Stewardship;
using MinistryPlatform.Models.DTO;

namespace crds_angular.Services
{
    public class DonorService : Interfaces.IDonorService
    {
        private readonly IDonorService _mpDonorService;
        private readonly IContactService _mpContactService;
        private readonly Interfaces.IPaymentService _paymentService;
        private readonly IAuthenticationService _authenticationService;

        private readonly string _guestGiverDisplayName;

        private readonly int _statementFrequencyNever;
        private readonly int _statementFrequencyQuarterly;
        private readonly int _statementTypeIndividual;
        private readonly int _statementMethodNone;
        private readonly int _statementMethodPostalMail;

        public DonorService(IDonorService mpDonorService, IContactService mpContactService,
            Interfaces.IPaymentService paymentService, IConfigurationWrapper configurationWrapper,
            IAuthenticationService authenticationService)
        {
            _mpDonorService = mpDonorService;
            _mpContactService = mpContactService;
            _paymentService = paymentService;
            _authenticationService = authenticationService;

            _guestGiverDisplayName = configurationWrapper.GetConfigValue("GuestGiverContactDisplayName");

            _statementFrequencyNever = configurationWrapper.GetConfigIntValue("DonorStatementFrequencyNever");
            _statementFrequencyQuarterly = configurationWrapper.GetConfigIntValue("DonorStatementFrequencyQuarterly");
            _statementTypeIndividual = configurationWrapper.GetConfigIntValue("DonorStatementTypeIndividual");
            _statementMethodNone = configurationWrapper.GetConfigIntValue("DonorStatementMethodNone");
            _statementMethodPostalMail = configurationWrapper.GetConfigIntValue("DonorStatementMethodPostalMail");
        }

        public ContactDonor GetContactDonorForEmail(string emailAddress)
        {
            return (_mpDonorService.GetPossibleGuestContactDonor(emailAddress));
        }

        public ContactDonor GetContactDonorForAuthenticatedUser(string authToken)
        {
            var contactId = _authenticationService.GetContactId(authToken);
            return (_mpDonorService.GetContactDonor(contactId));
        }

        public ContactDonor GetContactDonorForDonorAccount(string accountNumber, string routingNumber)
        {
            var acct = _mpDonorService.DecryptCheckValue(accountNumber);
            var rtn = _mpDonorService.DecryptCheckValue(routingNumber);
            return (_mpDonorService.GetContactDonorForDonorAccount(acct, rtn));
        }

        public ContactDonor GetContactDonorForCheckAccount(string encryptedKey)
        {
            return (_mpDonorService.GetContactDonorForCheckAccount(encryptedKey));
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
       ///  <param name="encryptedKey"> The encrypted routing and account number</param>
        /// <param name="emailAddress">An email address to use when creating a Contact (#1 above).</param>
        /// <param name="paymentProcessorToken">The one-time-use token given by the payment processor.</param>
        /// <param name="setupDate">The date when the Donor is marked as setup - normally would be today's date.</param>
        /// <returns></returns>
        public ContactDonor CreateOrUpdateContactDonor(ContactDonor contactDonor, string encryptedKey, string emailAddress, string paymentProcessorToken, DateTime setupDate)
        {
            var contactDonorResponse = new ContactDonor();
            StripeCustomer stripeCustomer = null;
            if (contactDonor == null || !contactDonor.ExistingContact)
            {
                var statementMethod = _statementMethodNone;
                var statementFrequency = _statementFrequencyNever;
                if (contactDonor != null && contactDonor.HasDetails)
                {
                    contactDonorResponse.ContactId = _mpContactService.CreateContactForNewDonor(contactDonor);
                    statementMethod = _statementMethodPostalMail;
                    statementFrequency = _statementFrequencyQuarterly;
                }
                else
                {
                    contactDonorResponse.ContactId = _mpContactService.CreateContactForGuestGiver(emailAddress, _guestGiverDisplayName);
                }

                stripeCustomer = _paymentService.CreateCustomer(paymentProcessorToken);

                var donorAccount = contactDonor != null ? contactDonor.Account : null;
                if (contactDonor != null)
                {
                    donorAccount.ProcessorAccountId = stripeCustomer.sources.data[0].id;
                }
           
                contactDonorResponse.ProcessorId = stripeCustomer.id;
                
                contactDonorResponse.DonorId = _mpDonorService.CreateDonorRecord(contactDonorResponse.ContactId, contactDonorResponse.ProcessorId, setupDate, 
                    statementFrequency, _statementTypeIndividual, statementMethod, donorAccount);
                contactDonorResponse.Email = emailAddress;
           
                _paymentService.UpdateCustomerDescription(contactDonorResponse.ProcessorId, contactDonorResponse.DonorId);
            }
            else if (!contactDonor.HasPaymentProcessorRecord)
            {
                contactDonorResponse.ContactId = contactDonor.ContactId;
                stripeCustomer = _paymentService.CreateCustomer(paymentProcessorToken);
                contactDonorResponse.ProcessorId = stripeCustomer.id;

                if (contactDonor.ExistingDonor)
                {
                    contactDonorResponse.DonorId = _mpDonorService.UpdatePaymentProcessorCustomerId(contactDonor.DonorId, contactDonorResponse.ProcessorId);
                }
                else
                {
                    if (contactDonor.RegisteredUser)
                    {
                        contactDonorResponse.DonorId = _mpDonorService.CreateDonorRecord(contactDonor.ContactId, contactDonorResponse.ProcessorId, setupDate);
                        var contact = _mpDonorService.GetEmailViaDonorId(contactDonorResponse.DonorId);
                        contactDonorResponse.Email = contact.Email;
                    }
                    else
                    {
                        contactDonorResponse.DonorId = _mpDonorService.CreateDonorRecord(contactDonor.ContactId, contactDonorResponse.ProcessorId, setupDate,
                            _statementFrequencyNever, _statementTypeIndividual, _statementMethodNone);
                    }
                }
                _paymentService.UpdateCustomerDescription(contactDonorResponse.ProcessorId, contactDonorResponse.DonorId);

                contactDonorResponse.RegisteredUser = contactDonor.RegisteredUser;
            }
            else
            {
                contactDonorResponse = contactDonor;
            }

            return (contactDonorResponse);
        }

        public string DecryptValues(string value)
        {
            return (_mpDonorService.DecryptCheckValue(value));
        }

        public string CreateRecurringGift(UpdateDonorDTO updateDonorDto)
        {
            var plan = _paymentService.CreatePlan(updateDonorDto);
            var subscription = _paymentService.CreateSubscription(plan.Id, "cus_74HHmDQARkpa9r");
            return null;
            // var recurGift = _mpDonorService.CreateRecurringGift(donorId);
            //var donorAcct = _mpDonorService.UpdateDonorAccount(null, sourceId, customerId);
        }

        public CreateDonationDistDto GetRecurringGiftForSubscription(string subscriptionId)
        {
            return (_mpDonorService.GetRecurringGiftForSubscription(subscriptionId));  
        }
    }
}