using crds_angular.Services;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using crds_angular.App_Start;
using crds_angular.Models.Crossroads.Stewardship;
using Crossroads.Utilities.Services;
using MinistryPlatform.Models.DTO;
using MinistryPlatform.Translation.Enum;
using RestSharp.Extensions;
using PaymentType = crds_angular.Models.Crossroads.Stewardship.PaymentType;

namespace crds_angular.test.Services
{
    public class DonorServiceTest
    {
        private DonorService _fixture;

        private Mock<MinistryPlatform.Translation.Services.Interfaces.IDonorService> _mpDonorService;
        private Mock<MinistryPlatform.Translation.Services.Interfaces.IContactService> _mpContactService;
        private Mock<crds_angular.Services.Interfaces.IPaymentService> _paymentService;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private Mock<MinistryPlatform.Translation.Services.Interfaces.IAuthenticationService> _authenticationService;

        private const string GuestGiverDisplayName = "Guest Giver";

        private const int StatementFrequencyNever = 9;
        private const int StatementFrequencyQuarterly = 99;
        private const int StatementTypeIndividual = 8;
        private const int StatementMethodNone = 7;
        private const int StatementMethodPostalMail = 77;
        private const string EncryptedKey =  "S/Hhsdgsydgs67733+jjdhjsdbnv332387uhrjfk";

        [SetUp]
        public void SetUp()
        {
            AutoMapperConfig.RegisterMappings();

            _mpDonorService = new Mock<MinistryPlatform.Translation.Services.Interfaces.IDonorService>(MockBehavior.Strict);
            _mpContactService = new Mock<MinistryPlatform.Translation.Services.Interfaces.IContactService>(MockBehavior.Strict);
            _paymentService = new Mock<crds_angular.Services.Interfaces.IPaymentService>(MockBehavior.Strict);
            _authenticationService = new Mock<MinistryPlatform.Translation.Services.Interfaces.IAuthenticationService>(MockBehavior.Strict);

            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("DonorStatementFrequencyNever")).Returns(StatementFrequencyNever);
            _configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("DonorStatementFrequencyQuarterly")).Returns(StatementFrequencyQuarterly);
            _configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("DonorStatementTypeIndividual")).Returns(StatementTypeIndividual);
            _configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("DonorStatementMethodNone")).Returns(StatementMethodNone);
            _configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("DonorStatementMethodPostalMail")).Returns(StatementMethodPostalMail);
            _configurationWrapper.Setup(mocked => mocked.GetConfigValue("GuestGiverContactDisplayName")).Returns(GuestGiverDisplayName);

            _fixture = new DonorService(_mpDonorService.Object, _mpContactService.Object, _paymentService.Object, _configurationWrapper.Object, _authenticationService.Object);

        }

        [Test]
        public void ShouldGetDonorForEmail()
        {
            var donor = new ContactDonor();
            _mpDonorService.Setup(mocked => mocked.GetPossibleGuestContactDonor("me@here.com")).Returns(donor);
            var response = _fixture.GetContactDonorForEmail("me@here.com");

            _mpDonorService.VerifyAll();
            Assert.AreSame(donor, response);
            Assert.IsFalse(response.RegisteredUser);
        }

        [Test]
        public void ShouldGetDonorForAuthenticatedUser()
        {
            var donor = new ContactDonor();
            _authenticationService.Setup(mocked => mocked.GetContactId("authToken")).Returns(123);
            _mpDonorService.Setup(mocked => mocked.GetContactDonor(123)).Returns(donor);
            var response = _fixture.GetContactDonorForAuthenticatedUser("authToken");

            _authenticationService.VerifyAll();
            _mpDonorService.VerifyAll();

            Assert.AreSame(donor, response);
        }

        [Test]
        public void ShouldReturnExistingDonorWithExistingStripeId()
        {
            var donor = new ContactDonor
            {
                ContactId = 12345,
                DonorId = 67890,
                ProcessorId = "Processor_ID",
                RegisteredUser = true
            };

            var response = _fixture.CreateOrUpdateContactDonor(donor, EncryptedKey, "me@here.com", "stripe_token", DateTime.Now);

            _mpDonorService.VerifyAll();
            _mpContactService.VerifyAll();
            _paymentService.VerifyAll();

            Assert.AreEqual(donor.ContactId, response.ContactId);
            Assert.AreEqual(donor.DonorId, response.DonorId);
            Assert.AreEqual(donor.ProcessorId, response.ProcessorId);
            Assert.AreEqual(donor.RegisteredUser, response.RegisteredUser);
        }

        [Test]
        public void ShouldCreateNewContactAndDonor()
        {
            var stripeCust = new StripeCustomer
            {
                id = "856",
                default_source = "123",
            };
            
            _mpContactService.Setup(mocked => mocked.CreateContactForGuestGiver("me@here.com", GuestGiverDisplayName)).Returns(123);
            _paymentService.Setup(mocked => mocked.CreateCustomer("stripe_token")).Returns(stripeCust);
            _mpDonorService.Setup(mocked => mocked.CreateDonorRecord(123, stripeCust.id, It.IsAny<DateTime>(), StatementFrequencyNever, StatementTypeIndividual, StatementMethodNone, null)).Returns(456);
            _paymentService.Setup(mocked => mocked.UpdateCustomerDescription(stripeCust.id, 456)).Returns("456");

            var response = _fixture.CreateOrUpdateContactDonor(null, EncryptedKey, "me@here.com", "stripe_token", DateTime.Now);

            _mpDonorService.VerifyAll();
            _mpContactService.VerifyAll();
            _paymentService.VerifyAll();

            Assert.AreEqual(123, response.ContactId);
            Assert.AreEqual(456, response.DonorId);
            Assert.AreEqual(stripeCust.id, response.ProcessorId);
            Assert.IsFalse(response.RegisteredUser);
        }

        [Test]
        public void ShouldCreateNewDonorForExistingContact()
        {
            var donor = new ContactDonor
            {
                ContactId = 12345,
                DonorId = 0,
            };

            var stripeCust = new StripeCustomer
            {
                id = "856",
                default_source = "123",
            };

            _paymentService.Setup(mocked => mocked.CreateCustomer("stripe_token")).Returns(stripeCust);
            _mpDonorService.Setup(mocked => mocked.CreateDonorRecord(12345, stripeCust.id, It.IsAny<DateTime>(), StatementFrequencyNever, StatementTypeIndividual, StatementMethodNone, null)).Returns(456);
            _paymentService.Setup(mocked => mocked.UpdateCustomerDescription(stripeCust.id, 456)).Returns("456");

            var response = _fixture.CreateOrUpdateContactDonor(donor, EncryptedKey, "me@here.com", "stripe_token", DateTime.Now);

            _mpDonorService.VerifyAll();
            _mpContactService.VerifyAll();
            _paymentService.VerifyAll();

            Assert.AreEqual(12345, response.ContactId);
            Assert.AreEqual(456, response.DonorId);
            Assert.AreEqual(stripeCust.id, response.ProcessorId);
        }

        [Test]
        public void ShouldCreateNewDonorForExistingRegisteredContact()
        {
            var donor = new ContactDonor
            {
                ContactId = 12345,
                DonorId = 0,
                RegisteredUser = true,
                Email = "me@here.com"
            };

            var stripeCust = new StripeCustomer
            {
                id = "856",
                default_source = "123",
            };

            _paymentService.Setup(mocked => mocked.CreateCustomer("stripe_token")).Returns(stripeCust);
            _mpDonorService.Setup(mocked => mocked.CreateDonorRecord(12345, stripeCust.id, It.IsAny<DateTime>(), 1, 1, 2, null)).Returns(456);
            _mpDonorService.Setup(mocked => mocked.GetEmailViaDonorId(456)).Returns(donor);
            _paymentService.Setup(mocked => mocked.UpdateCustomerDescription(stripeCust.id, 456)).Returns("456");

            var response = _fixture.CreateOrUpdateContactDonor(donor, EncryptedKey, "me@here.com", "stripe_token", DateTime.Now);

            _mpDonorService.VerifyAll();
            _mpContactService.VerifyAll();
            _paymentService.VerifyAll();

            Assert.AreEqual(12345, response.ContactId);
            Assert.AreEqual(456, response.DonorId);
            Assert.AreEqual(stripeCust.id, response.ProcessorId);
            Assert.AreEqual(donor.RegisteredUser, response.RegisteredUser);
        }

        [Test]
        public void ShouldUpdateExistingDonorForExistingContact()
        {
            var donor = new ContactDonor
            {
                ContactId = 12345,
                DonorId = 456,
            };

            var stripeCust = new StripeCustomer
            {
                id = "856",
                default_source = "123",
            };

            _paymentService.Setup(mocked => mocked.CreateCustomer("stripe_token")).Returns(stripeCust);
            _mpDonorService.Setup(mocked => mocked.UpdatePaymentProcessorCustomerId(456, stripeCust.id)).Returns(456);
            _paymentService.Setup(mocked => mocked.UpdateCustomerDescription(stripeCust.id, 456)).Returns("456");

            var response = _fixture.CreateOrUpdateContactDonor(donor, EncryptedKey, "me@here.com", "stripe_token", DateTime.Now);

            _mpDonorService.VerifyAll();
            _mpContactService.VerifyAll();
            _paymentService.VerifyAll();

            Assert.AreEqual(12345, response.ContactId);
            Assert.AreEqual(456, response.DonorId);
            Assert.AreEqual(stripeCust.id, response.ProcessorId);
        }

        public void TestCreateRecurringGift()
        {
            var recurringGiftDto = new RecurringGiftDto
            {
                StripeTokenId = "tok_123",
                PlanAmount = 123.45M,
                PlanInterval = PlanInterval.Weekly,
                Program = "987",
                StartDate = DateTime.Parse("1973-10-15")
            };

            var contactDonor = new ContactDonor
            {
                DonorId = 678,
                ProcessorId = "cus_123"
            };

            var stripeCustomer = new StripeCustomer
            {
                brand = "visa",
                last4 = "9876",
                id = "card_123"
            };

            var stripePlan = new StripePlan
            {
                Id = "plan_123"
            };

            const int donorAccountId = 999;

            var stripeSubscription = new StripeSubscription
            {
                Id = "sub_123"
            };

            const int recurringGiftId = 888;

            _paymentService.Setup(mocked => mocked.AddSourceToCustomer(contactDonor.ProcessorId, recurringGiftDto.StripeTokenId)).Returns(stripeCustomer);
            _paymentService.Setup(mocked => mocked.CreatePlan(recurringGiftDto, contactDonor)).Returns(stripePlan);
            _mpDonorService.Setup(
                mocked =>
                    mocked.CreateDonorAccount(stripeCustomer.brand,
                                              It.IsAny<string>(),
                                              stripeCustomer.last4,
                                              null,
                                              contactDonor.DonorId,
                                              stripeCustomer.id,
                                              contactDonor.ProcessorId)).Returns(donorAccountId);
            _paymentService.Setup(mocked => mocked.CreateSubscription(stripePlan.Id, contactDonor.ProcessorId)).Returns(stripeSubscription);
            _mpDonorService.Setup(
                mocked =>
                    mocked.CreateRecurringGiftRecord("auth", contactDonor.DonorId,
                                                     donorAccountId,
                                                     EnumMemberSerializationUtils.ToEnumString(recurringGiftDto.PlanInterval),
                                                     recurringGiftDto.PlanAmount,
                                                     recurringGiftDto.StartDate,
                                                     recurringGiftDto.Program,
                                                     stripeSubscription.Id)).Returns(recurringGiftId);
            var response = _fixture.CreateRecurringGift("auth", recurringGiftDto, contactDonor);
            _paymentService.VerifyAll();
            _mpDonorService.VerifyAll();
            Assert.AreEqual(recurringGiftId, response);
        }

        [Test]
        public void TestGetRecurringGiftsForAuthenticatedUser()
        {
            var records = new List<RecurringGift>
            {
                new RecurringGift
                {
                    RecurringGiftId = 123,
                    DonorID = 123123,
                    EmailAddress = "test@example.com",
                    Frequency = "Weekly",
                    Recurrence = "Fridays Weekly",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Amount = 950M,
                    ProgramID = 2,
                    ProgramName = "Beans & Rice",
                    CongregationName = "Uptown",
                    AccountTypeID = 3,
                    AccountNumberLast4= "4433",
                    InstitutionName = "Visa",
                    SubscriptionID = "sub_77L7hDGjQdoxRE",
                },
                new RecurringGift
                {
                    RecurringGiftId = 124,
                    DonorID = 123123,
                    EmailAddress = "test@example.com",
                    Frequency = "Monthly",
                    Recurrence = "8th Monthly",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Amount = 190M,
                    ProgramID = 1,
                    ProgramName = "Crossroads",
                    CongregationName = "",
                    AccountTypeID = 1,
                    AccountNumberLast4= "4093",
                    InstitutionName = "Bank",
                    SubscriptionID = "sub_77uaEIZLssR6xN",
                },
                new RecurringGift
                {
                    RecurringGiftId = 125,
                    DonorID = 123123,
                    EmailAddress = "test@example.com",
                    Frequency = "Weekly",
                    Recurrence = "Tuesdays Weekly",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Amount = 50M,
                    ProgramID = 3,
                    ProgramName = "Old St. George",
                    CongregationName = "General",
                    AccountTypeID = 3,
                    AccountNumberLast4= "1984",
                    InstitutionName = "American Express",
                    SubscriptionID = "sub_77L8qFUF6QFZsO",
                },
            };

            _mpDonorService.Setup(mocked => mocked.GetRecurringGiftsForAuthenticatedUser(It.IsAny<string>())).Returns(records);
            var result = _fixture.GetRecurringGiftsForAuthenticatedUser("afdafsaaatewjrtjeretewtr");

            _mpDonorService.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(records[0].RecurringGiftId, result[0].RecurringGiftId);
            Assert.AreEqual(records[0].DonorID, result[0].DonorID);
            Assert.AreEqual(records[0].EmailAddress, result[0].EmailAddress);
            Assert.AreEqual(records[0].Frequency.Matches("^.*Weekly") ? PlanInterval.Weekly : PlanInterval.Monthly, result[0].PlanInterval);
            Assert.AreEqual(records[0].Recurrence, result[0].Recurrence);
            Assert.AreEqual(records[0].StartDate, result[0].StartDate);
            Assert.AreEqual(records[0].EndDate, result[0].EndDate);
            Assert.AreEqual(records[0].Amount, result[0].PlanAmount);
            Assert.AreEqual(records[0].ProgramID.ToString(), result[0].Program);
            Assert.AreEqual(records[0].ProgramName, result[0].ProgramName);
            Assert.AreEqual(records[0].CongregationName, result[0].CongregationName);
            Assert.AreEqual(PaymentType.CreditCard, result[0].Source.SourceType);
            Assert.AreEqual(records[0].AccountNumberLast4, result[0].Source.AccountNumberLast4);
            Assert.AreEqual(CreditCardType.Visa, result[0].Source.CardType);
            Assert.AreEqual(records[0].SubscriptionID, result[0].SubscriptionID);

            Assert.AreEqual(records[1].RecurringGiftId, result[1].RecurringGiftId);
            Assert.AreEqual(records[1].DonorID, result[1].DonorID);
            Assert.AreEqual(records[1].EmailAddress, result[1].EmailAddress);
            Assert.AreEqual(records[1].Frequency.Matches("^.*Weekly") ? PlanInterval.Weekly : PlanInterval.Monthly, result[1].PlanInterval);
            Assert.AreEqual(records[1].Recurrence, result[1].Recurrence);
            Assert.AreEqual(records[1].StartDate, result[1].StartDate);
            Assert.AreEqual(records[1].EndDate, result[1].EndDate);
            Assert.AreEqual(records[1].Amount, result[1].PlanAmount);
            Assert.AreEqual(records[1].ProgramID.ToString(), result[1].Program);
            Assert.AreEqual(records[1].ProgramName, result[1].ProgramName);
            Assert.AreEqual(records[1].CongregationName, result[1].CongregationName);
            Assert.AreEqual(PaymentType.Bank, result[1].Source.SourceType);
            Assert.AreEqual(records[1].AccountNumberLast4, result[1].Source.AccountNumberLast4);
            Assert.AreEqual(null, result[1].Source.CardType);
            Assert.AreEqual(records[1].SubscriptionID, result[1].SubscriptionID);

            Assert.AreEqual(records[2].RecurringGiftId, result[2].RecurringGiftId);
            Assert.AreEqual(records[2].DonorID, result[2].DonorID);
            Assert.AreEqual(records[2].EmailAddress, result[2].EmailAddress);
            Assert.AreEqual(records[2].Frequency.Matches("^.*Weekly") ? PlanInterval.Weekly : PlanInterval.Monthly, result[2].PlanInterval);
            Assert.AreEqual(records[2].Recurrence, result[2].Recurrence);
            Assert.AreEqual(records[2].StartDate, result[2].StartDate);
            Assert.AreEqual(records[2].EndDate, result[2].EndDate);
            Assert.AreEqual(records[2].Amount, result[2].PlanAmount);
            Assert.AreEqual(records[2].ProgramID.ToString(), result[2].Program);
            Assert.AreEqual(records[2].ProgramName, result[2].ProgramName);
            Assert.AreEqual(records[2].CongregationName, result[2].CongregationName);
            Assert.AreEqual(PaymentType.CreditCard, result[2].Source.SourceType);
            Assert.AreEqual(records[2].AccountNumberLast4, result[2].Source.AccountNumberLast4);
            Assert.AreEqual(CreditCardType.AmericanExpress, result[2].Source.CardType);
            Assert.AreEqual(records[2].SubscriptionID, result[2].SubscriptionID);
        }

        [Test]
        public void TestCancelRecurringGift()
        {
            const string authUserToken = "auth";
            const int recurringGiftId = 123;
            var gift = new CreateDonationDistDto
            {
                DonorId = 456,
                SubscriptionId = "sub_123"
            };

            var contactDonor = new ContactDonor
            {
                ProcessorId = "cus_123"
            };

            var plan = new StripePlan
            {
                Id = "plan_123"
            };

            var subscription = new StripeSubscription
            {
                Plan = plan
            };

            _mpDonorService.Setup(mocked => mocked.GetRecurringGiftById(authUserToken, recurringGiftId)).Returns(gift);
            _mpDonorService.Setup(mocked => mocked.GetEmailViaDonorId(gift.DonorId)).Returns(contactDonor);
            _paymentService.Setup(mocked => mocked.CancelSubscription(contactDonor.ProcessorId, gift.SubscriptionId)).Returns(subscription);
            _paymentService.Setup(mocked => mocked.CancelPlan(subscription.Plan.Id)).Returns(plan);
            _mpDonorService.Setup(mocked => mocked.CancelRecurringGift(authUserToken, recurringGiftId));

            _fixture.CancelRecurringGift(authUserToken, recurringGiftId);
            _mpDonorService.VerifyAll();
            _paymentService.VerifyAll();
        }

        [Test]
        public void TestEditRecurringGiftNoEdits()
        {
            const string authUserToken = "auth";
            var today = DateTime.Today;

            var editGift = new RecurringGiftDto
            {
                RecurringGiftId = 345,
                StripeTokenId = string.Empty,
                PlanAmount = 500M,
                Program = "3",
                PlanInterval = PlanInterval.Weekly,
                StartDate = today
            };

            var donor = new ContactDonor
            {
                DonorId = 456
            };

            var existingGift = new CreateDonationDistDto
            {
                Amount = 50000,
                ProgramId = "3",
                Frequency = 1,
                StartDate = today,
                DonorAccountId = 234,
                SubscriptionId = "sub_123",
                DayOfWeek = (int)today.DayOfWeek,
                RecurringGiftId = 345
            };

            _mpDonorService.Setup(mocked => mocked.GetRecurringGiftById(authUserToken, editGift.RecurringGiftId)).Returns(existingGift);

            var result = _fixture.EditRecurringGift(authUserToken, editGift, donor);
            _mpDonorService.VerifyAll();
            _paymentService.VerifyAll();

            Assert.IsNotNull(result);
        }

        [Test]
        public void TestEditRecurringGiftOnlyChangePayment()
        {
            const string authUserToken = "auth";
            var today = DateTime.Today;

            var editGift = new RecurringGiftDto
            {
                RecurringGiftId = 345,
                StripeTokenId = "tok_123",
                PlanAmount = 500M,
                Program = "3",
                PlanInterval = PlanInterval.Weekly,
                StartDate = today
            };

            var donor = new ContactDonor
            {
                DonorId = 456,
                ProcessorId = "cus_123"
            };

            var existingGift = new CreateDonationDistDto
            {
                Amount = 50000,
                ProgramId = "3",
                Frequency = 1,
                StartDate = today,
                DonorAccountId = 234,
                SubscriptionId = "sub_123",
                DayOfWeek = (int)today.DayOfWeek,
                RecurringGiftId = 345,
                DonorId = 789
            };

            var stripeSource = new StripeCustomer
            {
                brand = "Visa",
                last4 = "1234",
                id = "card_123"
            };

            const int newDonorAccountId = 987;

            _mpDonorService.Setup(mocked => mocked.GetRecurringGiftById(authUserToken, editGift.RecurringGiftId)).Returns(existingGift);
            _paymentService.Setup(mocked => mocked.AddSourceToCustomer(donor.ProcessorId, editGift.StripeTokenId)).Returns(stripeSource);
            _mpDonorService.Setup(mocked => mocked.CreateDonorAccount(stripeSource.brand, "0", stripeSource.last4, null, existingGift.DonorId, stripeSource.id, donor.ProcessorId)).Returns(newDonorAccountId);
            _mpDonorService.Setup(mocked => mocked.UpdateRecurringGiftDonorAccount(authUserToken, existingGift.RecurringGiftId.Value, newDonorAccountId));

            var result = _fixture.EditRecurringGift(authUserToken, editGift, donor);
            _mpDonorService.VerifyAll();
            _paymentService.VerifyAll();

            Assert.IsNotNull(result);
        }

        [Test]
        public void TestEditRecurringGiftChangePaymentAndAmount()
        {
            const string authUserToken = "auth";
            var today = DateTime.Today;

            var editGift = new RecurringGiftDto
            {
                RecurringGiftId = 345,
                StripeTokenId = "tok_123",
                PlanAmount = 800M,
                Program = "3",
                PlanInterval = PlanInterval.Weekly,
                StartDate = today
            };

            var donor = new ContactDonor
            {
                DonorId = 456,
                ProcessorId = "cus_123",
                Email = "me@here.com"
            };

            var existingGift = new CreateDonationDistDto
            {
                Amount = 50000,
                ProgramId = "3",
                Frequency = 1,
                StartDate = today,
                DonorAccountId = 234,
                SubscriptionId = "sub_123",
                DayOfWeek = (int)today.DayOfWeek,
                RecurringGiftId = 345,
                DonorId = 789
            };

            var stripeSource = new StripeCustomer
            {
                brand = "Visa",
                last4 = "1234",
                id = "card_123"
            };

            const int newDonorAccountId = 987;

            var oldSubscription = new StripeSubscription
            {
                Plan = new StripePlan
                {
                    Id = "plan_123"
                }
            };

            var newPlan = new StripePlan
            {
                Id = "plan_456"
            };

            var newSubscription = new StripeSubscription
            {
                Id = "sub_456"
            };

            const int newRecurringGiftId = 765;

            var newRecurringGift = new CreateDonationDistDto
            {
                Amount = 80000,
                ProgramId = "3",
                Frequency = 1,
                StartDate = today,
                DonorAccountId = 234,
                SubscriptionId = "sub_456",
                DayOfWeek = (int)today.DayOfWeek,
                RecurringGiftId = newRecurringGiftId,
                DonorId = 789
            };

            _mpDonorService.Setup(mocked => mocked.GetRecurringGiftById(authUserToken, editGift.RecurringGiftId)).Returns(existingGift);
            _paymentService.Setup(mocked => mocked.AddSourceToCustomer(donor.ProcessorId, editGift.StripeTokenId)).Returns(stripeSource);
            _mpDonorService.Setup(mocked => mocked.CreateDonorAccount(stripeSource.brand, "0", stripeSource.last4, null, existingGift.DonorId, stripeSource.id, donor.ProcessorId)).Returns(newDonorAccountId);
            _paymentService.Setup(mocked => mocked.CancelSubscription(donor.ProcessorId, existingGift.SubscriptionId)).Returns(oldSubscription);
            _paymentService.Setup(mocked => mocked.CancelPlan(oldSubscription.Plan.Id)).Returns(oldSubscription.Plan);
            _paymentService.Setup(mocked => mocked.CreatePlan(editGift, donor)).Returns(newPlan);
            _paymentService.Setup(mocked => mocked.CreateSubscription(newPlan.Id, donor.ProcessorId)).Returns(newSubscription);
            _mpDonorService.Setup(mocked => mocked.CancelRecurringGift(authUserToken, existingGift.RecurringGiftId.Value));
            _mpDonorService.Setup(
                mocked =>
                    mocked.CreateRecurringGiftRecord(authUserToken,
                                                     donor.DonorId,
                                                     newDonorAccountId,
                                                     EnumMemberSerializationUtils.ToEnumString(editGift.PlanInterval),
                                                     editGift.PlanAmount,
                                                     editGift.StartDate,
                                                     editGift.Program,
                                                     newSubscription.Id)).Returns(newRecurringGiftId);
            _mpDonorService.Setup(mocked => mocked.GetRecurringGiftById(authUserToken, newRecurringGiftId)).Returns(newRecurringGift);

            var result = _fixture.EditRecurringGift(authUserToken, editGift, donor);
            _mpDonorService.VerifyAll();
            _paymentService.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(newRecurringGift.RecurringGiftId, result.RecurringGiftId);
            Assert.AreEqual(newRecurringGift.StartDate, result.StartDate);
            Assert.AreEqual(newRecurringGift.Amount, result.PlanAmount);
            Assert.AreEqual(PlanInterval.Weekly, result.PlanInterval);
            Assert.AreEqual(newRecurringGift.ProgramId, result.Program);
            Assert.AreEqual(newRecurringGift.DonorId, result.DonorID);
            Assert.AreEqual(donor.Email, result.EmailAddress);
            Assert.AreEqual(newSubscription.Id, result.SubscriptionID);
        }
    }
}
