using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using crds_angular.Models.Crossroads.Stewardship;
using MinistryPlatform.Models.DTO;
using Crossroads.Utilities;
using Crossroads.Utilities.Services;

namespace crds_angular.Services
{
    public class DonorService : Interfaces.IDonorService
    {
        private readonly IDonorService _mpDonorService;
        private readonly IContactService _mpContactService;
        private readonly Interfaces.IPaymentService _paymentService;
        private readonly IAuthenticationService _authenticationService;
        public const string DefaultInstitutionName = "Bank";
        public const string DonorRoutingNumberDefault = "0";
        public const string DonorAccountNumberDefault = "0";
        
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

        public ContactDonor GetContactDonorForDonorId(int donorId)
        {
            return (_mpDonorService.GetEmailViaDonorId(donorId));
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
                if (donorAccount != null)
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

        public int CreateRecurringGift(string authorizedUserToken, RecurringGiftDto recurringGiftDto, ContactDonor contactDonor)
        {
            var response = _paymentService.AddSourceToCustomer(contactDonor.ProcessorId, recurringGiftDto.StripeTokenId);

            var plan = _paymentService.CreatePlan(recurringGiftDto, contactDonor);

            var donorAccountId = _mpDonorService.CreateDonorAccount(response.brand,
                                                           DonorRoutingNumberDefault,
                                                           response.last4,
                                                           null,
                                                           contactDonor.DonorId,
                                                           response.id,
                                                           contactDonor.ProcessorId);
            var stripeSubscription = _paymentService.CreateSubscription(plan.Id, contactDonor.ProcessorId);
           
            var recurGiftId = _mpDonorService.CreateRecurringGiftRecord(authorizedUserToken, contactDonor.DonorId,
                                                                donorAccountId,
                                                                EnumMemberSerializationUtils.ToEnumString(recurringGiftDto.PlanInterval),
                                                                recurringGiftDto.PlanAmount,
                                                                recurringGiftDto.StartDate,
                                                                recurringGiftDto.Program,
                                                                stripeSubscription.Id);
            return recurGiftId;
        }

        public void CancelRecurringGift(string authorizedUserToken, int recurringGiftId)
        {
            var existingGift = _mpDonorService.GetRecurringGiftById(authorizedUserToken, recurringGiftId);
            var donor = GetContactDonorForDonorId(existingGift.DonorId);

            var subscription = _paymentService.CancelSubscription(donor.ProcessorId, existingGift.SubscriptionId);
            _paymentService.CancelPlan(subscription.Plan.Id);

            _mpDonorService.CancelRecurringGift(authorizedUserToken, recurringGiftId);
        }

        public RecurringGiftDto EditRecurringGift(string authorizedUserToken, RecurringGiftDto editGift, ContactDonor donor)
        {
            var existingGift = _mpDonorService.GetRecurringGiftById(authorizedUserToken, editGift.RecurringGiftId);

            // Assuming payment info is changed if a token is given.
            var changedPayment = !string.IsNullOrWhiteSpace(editGift.StripeTokenId);

            var changedAmount = (int)(editGift.PlanAmount * Constants.StripeDecimalConversionValue) != existingGift.Amount;
            var changedProgram = !editGift.Program.Equals(existingGift.ProgramId);
            var changedFrequency = !editGift.PlanInterval.Equals(existingGift.Frequency == 1 ? PlanInterval.Weekly : PlanInterval.Monthly);
            var changedDayOfWeek = changedFrequency || (editGift.PlanInterval == PlanInterval.Weekly && (int) editGift.StartDate.DayOfWeek != existingGift.DayOfWeek);
            var changedDayOfMonth = changedFrequency || (editGift.PlanInterval == PlanInterval.Monthly && editGift.StartDate.Day != existingGift.DayOfMonth);
            var changedStartDate = editGift.StartDate.Date != existingGift.StartDate.Value.Date;

            var needsNewStripePlan = changedAmount ||changedFrequency || changedDayOfWeek || changedDayOfMonth || changedStartDate;
            var needsNewMpRecurringGift = changedAmount || changedProgram || needsNewStripePlan;

            var recurringGiftId = existingGift.RecurringGiftId.GetValueOrDefault(-1);

            int donorAccountId;
            if (changedPayment)
            {
                var customer = _paymentService.AddSourceToCustomer(donor.ProcessorId, editGift.StripeTokenId);
                // TODO Need to update source on Subscription/Customer in Stripe - depends on solution for DE494

                // TODO Need to change this to accept a user's token in order to facilitate Admin edit
                donorAccountId = _mpDonorService.CreateDonorAccount(authorizedUserToken,
                                                                    customer.brand,
                                                                    DonorRoutingNumberDefault,
                                                                    customer.last4,
                                                                    existingGift.DonorId,
                                                                    customer.id,
                                                                    donor.ProcessorId);
                _mpDonorService.UpdateRecurringGiftDonorAccount(authorizedUserToken, recurringGiftId, donorAccountId);
            }
            else
            {
                donorAccountId = existingGift.DonorAccountId.Value;
            }

            var stripeSubscription = new StripeSubscription {Id = existingGift.SubscriptionId};

            if (needsNewMpRecurringGift)
            {
                if (needsNewStripePlan)
                {
                    var oldSubscription = _paymentService.CancelSubscription(donor.ProcessorId, stripeSubscription.Id);
                    _paymentService.CancelPlan(oldSubscription.Plan.Id);

                    var plan = _paymentService.CreatePlan(editGift, donor);
                    stripeSubscription = _paymentService.CreateSubscription(plan.Id, donor.ProcessorId);
                }

                _mpDonorService.CancelRecurringGift(authorizedUserToken, recurringGiftId);

                recurringGiftId = _mpDonorService.CreateRecurringGiftRecord(authorizedUserToken,
                                                                            donor.DonorId,
                                                                            donorAccountId,
                                                                            EnumMemberSerializationUtils.ToEnumString(editGift.PlanInterval),
                                                                            editGift.PlanAmount,
                                                                            editGift.StartDate,
                                                                            editGift.Program,
                                                                            stripeSubscription.Id);

            }

            var newGift = _mpDonorService.GetRecurringGiftById(authorizedUserToken, recurringGiftId);

            var newRecurringGift = new RecurringGiftDto
            {
                RecurringGiftId = newGift.RecurringGiftId.Value,
                StartDate = newGift.StartDate.Value,
                PlanAmount = newGift.Amount,
                PlanInterval = newGift.Frequency == 1 ? PlanInterval.Weekly : PlanInterval.Monthly,
                Program = newGift.ProgramId,
                DonorID = newGift.DonorId,
                EmailAddress = donor.Email,
                SubscriptionID = stripeSubscription.Id
            };
            return (newRecurringGift);
        }

        public CreateDonationDistDto GetRecurringGiftForSubscription(string subscriptionId)
        {
            return (_mpDonorService.GetRecurringGiftForSubscription(subscriptionId));  
        }

        public List<RecurringGiftDto> GetRecurringGiftsForAuthenticatedUser(string userToken)
        {
            var records = _mpDonorService.GetRecurringGiftsForAuthenticatedUser(userToken);
            return records.Select(Mapper.Map<RecurringGift, RecurringGiftDto>).ToList();
        }
    }
}