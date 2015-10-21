using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models.DTO;
using Crossroads.Utilities;
using Crossroads.Utilities.Services;
using log4net;
using IDonorService = MinistryPlatform.Translation.Services.Interfaces.IDonorService;

namespace crds_angular.Services
{
    public class DonorService : Interfaces.IDonorService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof (DonorService));

        private readonly IDonorService _mpDonorService;
        private readonly IContactService _mpContactService;
        private readonly Interfaces.IPaymentService _paymentService;
        private readonly IAuthenticationService _authenticationService;
        public const string DefaultInstitutionName = "Bank";
        public const string DonorRoutingNumberDefault = "0";
        public const string DonorAccountNumberDefault = "0";
        public const int RecurringGiftFrequencyWeekly = 1;
        
        private readonly string _guestGiverDisplayName;

        private readonly int _statementFrequencyNever;
        private readonly int _statementFrequencyQuarterly;
        private readonly int _statementTypeIndividual;
        private readonly int _statementMethodNone;
        private readonly int _statementMethodPostalMail;
        private readonly int _notSiteSpecificCongregation;

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
            _notSiteSpecificCongregation = configurationWrapper.GetConfigIntValue("NotSiteSpecificCongreagtion");
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
        /// <param name="createPaymentProcessorCustomer">Create a payment processor customer record.  Defaults to true.</param>
        /// <returns></returns>
        public ContactDonor CreateOrUpdateContactDonor(ContactDonor contactDonor, string encryptedKey, string emailAddress, string paymentProcessorToken, DateTime setupDate /*, bool createPaymentProcessorCustomer = true */)
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

                var donorAccount = contactDonor != null ? contactDonor.Account : null;
                //if (createPaymentProcessorCustomer)
                //{
                    stripeCustomer = _paymentService.CreateCustomer(paymentProcessorToken);
                    if (donorAccount != null)
                    {
                        donorAccount.ProcessorAccountId = stripeCustomer.sources.data[0].id;
                    }
                    contactDonorResponse.ProcessorId = stripeCustomer.id;
                //}

           
                contactDonorResponse.DonorId = _mpDonorService.CreateDonorRecord(contactDonorResponse.ContactId, contactDonorResponse.ProcessorId, setupDate, 
                    statementFrequency, _statementTypeIndividual, statementMethod, donorAccount);
                contactDonorResponse.Email = emailAddress;

                //if (createPaymentProcessorCustomer)
                //{
                    _paymentService.UpdateCustomerDescription(contactDonorResponse.ProcessorId, contactDonorResponse.DonorId);
                //}
            }
            else if (!contactDonor.HasPaymentProcessorRecord)
            {
                contactDonorResponse.ContactId = contactDonor.ContactId;
                //if (createPaymentProcessorCustomer)
                //{
                    stripeCustomer = _paymentService.CreateCustomer(paymentProcessorToken);
                    contactDonorResponse.ProcessorId = stripeCustomer.id;
                //}

                if (contactDonor.ExistingDonor)
                {
                    //if (createPaymentProcessorCustomer)
                    //{
                        contactDonorResponse.DonorId = _mpDonorService.UpdatePaymentProcessorCustomerId(contactDonor.DonorId, contactDonorResponse.ProcessorId);
                    //}
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

                //if (createPaymentProcessorCustomer)
                //{
                    _paymentService.UpdateCustomerDescription(contactDonorResponse.ProcessorId, contactDonorResponse.DonorId);
                //}

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
            StripeCustomer customer = null;
            StripePlan plan = null;
            StripeSubscription stripeSubscription = null;
            var donorAccountId = -1;
            var recurGiftId = -1;

            try
            {

                customer = _paymentService.CreateCustomer(recurringGiftDto.StripeTokenId, string.Format("{0}, Recurring Gift Subscription", contactDonor.DonorId));

                var source = customer.sources.data.Find(s => s.id == customer.default_source);

                donorAccountId = _mpDonorService.CreateDonorAccount(source.brand,
                                                                        DonorRoutingNumberDefault,
                                                                        source.last4,
                                                                        null,
                                                                        contactDonor.DonorId,
                                                                        source.id,
                                                                        customer.id);

                plan = _paymentService.CreatePlan(recurringGiftDto, contactDonor);

                stripeSubscription = _paymentService.CreateSubscription(plan.Id, customer.id);

                var contact = _mpContactService.GetContactById(contactDonor.ContactId);
                var congregation = contact.Congregation_ID ?? _notSiteSpecificCongregation;

                recurGiftId = _mpDonorService.CreateRecurringGiftRecord(authorizedUserToken,
                                                                            contactDonor.DonorId,
                                                                            donorAccountId,
                                                                            EnumMemberSerializationUtils.ToEnumString(recurringGiftDto.PlanInterval),
                                                                            recurringGiftDto.PlanAmount,
                                                                            recurringGiftDto.StartDate,
                                                                            recurringGiftDto.Program,
                                                                            stripeSubscription.Id,
                                                                            congregation);
                return recurGiftId;
            }
            catch (Exception e)
            {
                // "Rollback" any updates
                _logger.Warn(string.Format("Error setting up recurring gift for donor {0}", contactDonor.DonorId), e);
                if (stripeSubscription != null)
                {
                    _logger.Debug(string.Format("Deleting Stripe Subscription {0} for donor {1}", stripeSubscription.Id, contactDonor.DonorId));
                    _paymentService.CancelSubscription(customer.id, stripeSubscription.Id);
                }

                if (plan != null)
                {
                    _logger.Debug(string.Format("Deleting Stripe Plan {0} for donor {1}", plan.Id, contactDonor.DonorId));
                    _paymentService.CancelPlan(plan.Id);
                }

                if (customer != null)
                {
                    _logger.Debug(string.Format("Deleting Stripe Cubscription {0} for donor {1}", customer.id, contactDonor.DonorId));
                    _paymentService.DeleteCustomer(customer.id);
                }

                if (recurGiftId != -1)
                {
                    _mpDonorService.CancelRecurringGift(authorizedUserToken, recurGiftId);
                }

                if (donorAccountId != -1)
                {
                    _mpDonorService.DeleteDonorAccount(authorizedUserToken, donorAccountId);
                }

                throw;
            }
        }

        public void CancelRecurringGift(string authorizedUserToken, int recurringGiftId)
        {
            var existingGift = _mpDonorService.GetRecurringGiftById(authorizedUserToken, recurringGiftId);
            var donor = GetContactDonorForDonorId(existingGift.DonorId);

            var subscription = _paymentService.CancelSubscription(donor.ProcessorId, existingGift.SubscriptionId);
            _paymentService.CancelPlan(subscription.Plan.Id);

            _mpDonorService.CancelRecurringGift(authorizedUserToken, recurringGiftId);
        }

        /// <summary>
        /// Edit an existing recurring gift.  This will cancel (end-date) an existing RecurringGift and then create a new one
        /// if the Program, Amount, Frequency, Day of Week, or Day of Month are changed.  This will simply edit the existing gift
        /// if only the payment method (credit card, bank account) is changed.
        /// </summary>
        /// <param name="authorizedUserToken">An OAuth token for the user who is logged in to cr.net/MP</param>
        /// <param name="editGift">The edited values for the Recurring Gift</param>
        /// <param name="donor">The donor performing the edits</param>
        /// <returns>A RecurringGiftDto, populated with any new/updated values after any edits</returns>
        public RecurringGiftDto EditRecurringGift(string authorizedUserToken, RecurringGiftDto editGift, ContactDonor donor)
        {
            var existingGift = _mpDonorService.GetRecurringGiftById(authorizedUserToken, editGift.RecurringGiftId);

            // Assuming payment info is changed if a token is given.
            var changedPayment = !string.IsNullOrWhiteSpace(editGift.StripeTokenId);

            var changedAmount = (int)(editGift.PlanAmount * Constants.StripeDecimalConversionValue) != existingGift.Amount;
            var changedProgram = !editGift.Program.Equals(existingGift.ProgramId);
            var changedFrequency = !editGift.PlanInterval.Equals(existingGift.Frequency == RecurringGiftFrequencyWeekly ? PlanInterval.Weekly : PlanInterval.Monthly);
            var changedDayOfWeek = changedFrequency || (editGift.PlanInterval == PlanInterval.Weekly && (int) editGift.StartDate.DayOfWeek != existingGift.DayOfWeek);
            var changedDayOfMonth = changedFrequency || (editGift.PlanInterval == PlanInterval.Monthly && editGift.StartDate.Day != existingGift.DayOfMonth);
            var changedStartDate = editGift.StartDate.Date != existingGift.StartDate.GetValueOrDefault().Date;

            var needsNewStripePlan = changedAmount ||changedFrequency || changedDayOfWeek || changedDayOfMonth || changedStartDate;
            var needsNewMpRecurringGift = changedAmount || changedProgram || needsNewStripePlan;

            var recurringGiftId = existingGift.RecurringGiftId.GetValueOrDefault(-1);

            int donorAccountId;

            if (changedPayment)
            {
                // If the payment method changed, we need to create a new Stripe Source.
                var source = _paymentService.AddSourceToCustomer(donor.ProcessorId, editGift.StripeTokenId);
                // TODO Need to update source on Subscription/Customer in Stripe - depends on solution for DE494

                donorAccountId = _mpDonorService.CreateDonorAccount(source.brand,
                                                                    DonorRoutingNumberDefault,
                                                                    source.last4,
                                                                    null, //Encrypted account
                                                                    existingGift.DonorId,
                                                                    source.id,
                                                                    donor.ProcessorId);

                // If we are not going to create a new Recurring Gift, then we'll update the existing
                // gift with the new donor account
                if (!needsNewMpRecurringGift)
                {
                    _mpDonorService.UpdateRecurringGiftDonorAccount(authorizedUserToken, recurringGiftId, donorAccountId);
                }
            }
            else
            {
                // If the payment method is not changed, set the donorAccountId with the existing ID so we can use it later
                donorAccountId = existingGift.DonorAccountId.GetValueOrDefault();
            }

            // Initialize a StripeSubscription, as we need the ID later on
            var stripeSubscription = new StripeSubscription {Id = existingGift.SubscriptionId};

            if (needsNewMpRecurringGift)
            {
                if (needsNewStripePlan)
                {
                    // If we need a new Stripe Plan, cancel the old subscription and plan, and create a new one
                    var oldSubscription = _paymentService.CancelSubscription(donor.ProcessorId, stripeSubscription.Id);
                    _paymentService.CancelPlan(oldSubscription.Plan.Id);

                    var plan = _paymentService.CreatePlan(editGift, donor);
                    stripeSubscription = _paymentService.CreateSubscription(plan.Id, donor.ProcessorId);
                }

                // Cancel the old recurring gift, and create a new one
                _mpDonorService.CancelRecurringGift(authorizedUserToken, recurringGiftId);
                var contact = _mpContactService.GetContactById(donor.ContactId);
                var congregation = contact.Congregation_ID ?? 5;

                recurringGiftId = _mpDonorService.CreateRecurringGiftRecord(authorizedUserToken,
                                                                            donor.DonorId,
                                                                            donorAccountId,
                                                                            EnumMemberSerializationUtils.ToEnumString(editGift.PlanInterval),
                                                                            editGift.PlanAmount,
                                                                            editGift.StartDate,
                                                                            editGift.Program,
                                                                            stripeSubscription.Id,
                                                                            congregation);

            }

            // Get the new/updated recurring gift so we can return a DTO with all the new values
            var newGift = _mpDonorService.GetRecurringGiftById(authorizedUserToken, recurringGiftId);

            var newRecurringGift = new RecurringGiftDto
            {
                RecurringGiftId = newGift.RecurringGiftId.GetValueOrDefault(),
                StartDate = newGift.StartDate.GetValueOrDefault(),
                PlanAmount = newGift.Amount,
                PlanInterval = newGift.Frequency == RecurringGiftFrequencyWeekly ? PlanInterval.Weekly : PlanInterval.Monthly,
                Program = newGift.ProgramId,
                DonorID = newGift.DonorId,
                EmailAddress = donor.Email,
                SubscriptionID = stripeSubscription.Id,
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
            var recurringGifts = records.Select(Mapper.Map<RecurringGift, RecurringGiftDto>).ToList();

            // We're not currently storing routing number, postal code, or expiration date in the DonorAccount table.
            // We need these for editing a gift, so populate them from Stripe
            foreach (var gift in recurringGifts)
            {
                PopulateStripeInfoOnRecurringGiftSource(gift.Source);
            }

            return (recurringGifts);
        }

        private void PopulateStripeInfoOnRecurringGiftSource(DonationSourceDTO donationSource)
        {
            var source = _paymentService.GetSource(donationSource.PaymentProcessorId, donationSource.ProcessorAccountId);
            if (source == null)
            {
                return;
            }

            donationSource.PostalCode = source.address_zip;
            donationSource.RoutingNumber = source.routing_number;
            if (!string.IsNullOrWhiteSpace(source.exp_month) && !string.IsNullOrWhiteSpace(source.exp_year))
            {
                donationSource.ExpirationDate = DateTime.ParseExact(string.Format("{0}/01/{1}", source.exp_month, source.exp_year), "M/dd/yyyy", DateTimeFormatInfo.CurrentInfo);
            }
        }
    }
}