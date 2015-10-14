using crds_angular.Services;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using crds_angular.App_Start;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads.Stewardship;
using Crossroads.Utilities.Services;
using MinistryPlatform.Models.DTO;
using RestSharp.Extensions;

namespace crds_angular.test.Services
{
    public class DonorServiceTest
    {
        private DonorService fixture;

        private Mock<MinistryPlatform.Translation.Services.Interfaces.IDonorService> mpDonorService;
        private Mock<MinistryPlatform.Translation.Services.Interfaces.IContactService> mpContactService;
        private Mock<crds_angular.Services.Interfaces.IPaymentService> paymentService;
        private Mock<IConfigurationWrapper> configurationWrapper;
        private Mock<MinistryPlatform.Translation.Services.Interfaces.IAuthenticationService> authenticationService;

        private const string GUEST_GIVER_DISPLAY_NAME = "Guest Giver";

        private const int STATEMENT_FREQUENCY_NEVER = 9;
        private const int STATEMENT_FREQUENCY_QUARTERLY = 99;
        private const int STATEMENT_TYPE_INDIVIDUAL = 8;
        private const int STATEMENT_METHOD_NONE = 7;
        private const int STATEMENT_METHOD_POSTAL_MAIL = 77;
        private const string ENCRYPTED_KEY =  "S/Hhsdgsydgs67733+jjdhjsdbnv332387uhrjfk";

        [SetUp]
        public void SetUp()
        {
            AutoMapperConfig.RegisterMappings();

            mpDonorService = new Mock<MinistryPlatform.Translation.Services.Interfaces.IDonorService>(MockBehavior.Strict);
            mpContactService = new Mock<MinistryPlatform.Translation.Services.Interfaces.IContactService>(MockBehavior.Strict);
            paymentService = new Mock<crds_angular.Services.Interfaces.IPaymentService>(MockBehavior.Strict);
            authenticationService = new Mock<MinistryPlatform.Translation.Services.Interfaces.IAuthenticationService>(MockBehavior.Strict);

            configurationWrapper = new Mock<IConfigurationWrapper>();
            configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("DonorStatementFrequencyNever")).Returns(STATEMENT_FREQUENCY_NEVER);
            configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("DonorStatementFrequencyQuarterly")).Returns(STATEMENT_FREQUENCY_QUARTERLY);
            configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("DonorStatementTypeIndividual")).Returns(STATEMENT_TYPE_INDIVIDUAL);
            configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("DonorStatementMethodNone")).Returns(STATEMENT_METHOD_NONE);
            configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("DonorStatementMethodPostalMail")).Returns(STATEMENT_METHOD_POSTAL_MAIL);
            configurationWrapper.Setup(mocked => mocked.GetConfigValue("GuestGiverContactDisplayName")).Returns(GUEST_GIVER_DISPLAY_NAME);

            fixture = new DonorService(mpDonorService.Object, mpContactService.Object, paymentService.Object, configurationWrapper.Object, authenticationService.Object);

        }

        [Test]
        public void shouldGetDonorForEmail()
        {
            var donor = new ContactDonor();
            mpDonorService.Setup(mocked => mocked.GetPossibleGuestContactDonor("me@here.com")).Returns(donor);
            var response = fixture.GetContactDonorForEmail("me@here.com");

            mpDonorService.VerifyAll();
            Assert.AreSame(donor, response);
            Assert.IsFalse(response.RegisteredUser);
        }

        [Test]
        public void shouldGetDonorForAuthenticatedUser()
        {
            var donor = new ContactDonor();
            authenticationService.Setup(mocked => mocked.GetContactId("authToken")).Returns(123);
            mpDonorService.Setup(mocked => mocked.GetContactDonor(123)).Returns(donor);
            var response = fixture.GetContactDonorForAuthenticatedUser("authToken");

            authenticationService.VerifyAll();
            mpDonorService.VerifyAll();

            Assert.AreSame(donor, response);
        }

        [Test]
        public void shouldReturnExistingDonorWithExistingStripeId()
        {
            var donor = new ContactDonor
            {
                ContactId = 12345,
                DonorId = 67890,
                ProcessorId = "Processor_ID",
                RegisteredUser = true
            };

            var response = fixture.CreateOrUpdateContactDonor(donor, ENCRYPTED_KEY, "me@here.com", "stripe_token", DateTime.Now);

            mpDonorService.VerifyAll();
            mpContactService.VerifyAll();
            paymentService.VerifyAll();

            Assert.AreEqual(donor.ContactId, response.ContactId);
            Assert.AreEqual(donor.DonorId, response.DonorId);
            Assert.AreEqual(donor.ProcessorId, response.ProcessorId);
            Assert.AreEqual(donor.RegisteredUser, response.RegisteredUser);
        }

        [Test]
        public void shouldCreateNewContactAndDonor()
        {
            var stripeCust = new StripeCustomer
            {
                id = "856",
                default_source = "123",
            };
            
            mpContactService.Setup(mocked => mocked.CreateContactForGuestGiver("me@here.com", GUEST_GIVER_DISPLAY_NAME)).Returns(123);
            paymentService.Setup(mocked => mocked.CreateCustomer("stripe_token")).Returns(stripeCust);
            mpDonorService.Setup(mocked => mocked.CreateDonorRecord(123, stripeCust.id, It.IsAny<DateTime>(), STATEMENT_FREQUENCY_NEVER, STATEMENT_TYPE_INDIVIDUAL, STATEMENT_METHOD_NONE, null)).Returns(456);
            paymentService.Setup(mocked => mocked.UpdateCustomerDescription(stripeCust.id, 456)).Returns("456");

            var response = fixture.CreateOrUpdateContactDonor(null, ENCRYPTED_KEY, "me@here.com", "stripe_token", DateTime.Now);

            mpDonorService.VerifyAll();
            mpContactService.VerifyAll();
            paymentService.VerifyAll();

            Assert.AreEqual(123, response.ContactId);
            Assert.AreEqual(456, response.DonorId);
            Assert.AreEqual(stripeCust.id, response.ProcessorId);
            Assert.IsFalse(response.RegisteredUser);
        }

        [Test]
        public void shouldCreateNewDonorForExistingContact()
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

            paymentService.Setup(mocked => mocked.CreateCustomer("stripe_token")).Returns(stripeCust);
            mpDonorService.Setup(mocked => mocked.CreateDonorRecord(12345, stripeCust.id, It.IsAny<DateTime>(), STATEMENT_FREQUENCY_NEVER, STATEMENT_TYPE_INDIVIDUAL, STATEMENT_METHOD_NONE, null)).Returns(456);
            paymentService.Setup(mocked => mocked.UpdateCustomerDescription(stripeCust.id, 456)).Returns("456");

            var response = fixture.CreateOrUpdateContactDonor(donor, ENCRYPTED_KEY, "me@here.com", "stripe_token", DateTime.Now);

            mpDonorService.VerifyAll();
            mpContactService.VerifyAll();
            paymentService.VerifyAll();

            Assert.AreEqual(12345, response.ContactId);
            Assert.AreEqual(456, response.DonorId);
            Assert.AreEqual(stripeCust.id, response.ProcessorId);
        }

        [Test]
        public void shouldCreateNewDonorForExistingRegisteredContact()
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

            paymentService.Setup(mocked => mocked.CreateCustomer("stripe_token")).Returns(stripeCust);
            mpDonorService.Setup(mocked => mocked.CreateDonorRecord(12345, stripeCust.id, It.IsAny<DateTime>(), 1, 1, 2, null)).Returns(456);
            mpDonorService.Setup(mocked => mocked.GetEmailViaDonorId(456)).Returns(donor);
            paymentService.Setup(mocked => mocked.UpdateCustomerDescription(stripeCust.id, 456)).Returns("456");

            var response = fixture.CreateOrUpdateContactDonor(donor, ENCRYPTED_KEY, "me@here.com", "stripe_token", DateTime.Now);

            mpDonorService.VerifyAll();
            mpContactService.VerifyAll();
            paymentService.VerifyAll();

            Assert.AreEqual(12345, response.ContactId);
            Assert.AreEqual(456, response.DonorId);
            Assert.AreEqual(stripeCust.id, response.ProcessorId);
            Assert.AreEqual(donor.RegisteredUser, response.RegisteredUser);
        }

        [Test]
        public void shouldUpdateExistingDonorForExistingContact()
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

            paymentService.Setup(mocked => mocked.CreateCustomer("stripe_token")).Returns(stripeCust);
            mpDonorService.Setup(mocked => mocked.UpdatePaymentProcessorCustomerId(456, stripeCust.id)).Returns(456);
            paymentService.Setup(mocked => mocked.UpdateCustomerDescription(stripeCust.id, 456)).Returns("456");

            var response = fixture.CreateOrUpdateContactDonor(donor, ENCRYPTED_KEY, "me@here.com", "stripe_token", DateTime.Now);

            mpDonorService.VerifyAll();
            mpContactService.VerifyAll();
            paymentService.VerifyAll();

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

            paymentService.Setup(mocked => mocked.AddSourceToCustomer(contactDonor.ProcessorId, recurringGiftDto.StripeTokenId)).Returns(stripeCustomer);
            paymentService.Setup(mocked => mocked.CreatePlan(recurringGiftDto, contactDonor)).Returns(stripePlan);
            mpDonorService.Setup(
                mocked =>
                    mocked.CreateDonorAccount(stripeCustomer.brand,
                                              It.IsAny<string>(),
                                              stripeCustomer.last4,
                                              null,
                                              contactDonor.DonorId,
                                              stripeCustomer.id,
                                              contactDonor.ProcessorId)).Returns(donorAccountId);
            paymentService.Setup(mocked => mocked.CreateSubscription(stripePlan.Id, contactDonor.ProcessorId)).Returns(stripeSubscription);
            mpDonorService.Setup(
                mocked =>
                    mocked.CreateRecurringGiftRecord("auth", contactDonor.DonorId,
                                                     donorAccountId,
                                                     EnumMemberSerializationUtils.ToEnumString(recurringGiftDto.PlanInterval),
                                                     recurringGiftDto.PlanAmount,
                                                     recurringGiftDto.StartDate,
                                                     recurringGiftDto.Program,
                                                     stripeSubscription.Id)).Returns(recurringGiftId);
            var response = fixture.CreateRecurringGift("auth", recurringGiftDto, contactDonor);
            paymentService.VerifyAll();
            mpDonorService.VerifyAll();
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

            mpDonorService.Setup(mocked => mocked.GetRecurringGiftsForAuthenticatedUser(It.IsAny<string>())).Returns(records);
            var result = fixture.GetRecurringGiftsForAuthenticatedUser("afdafsaaatewjrtjeretewtr");

            mpDonorService.VerifyAll();

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

            mpDonorService.Setup(mocked => mocked.GetRecurringGiftById(authUserToken, recurringGiftId)).Returns(gift);
            mpDonorService.Setup(mocked => mocked.GetEmailViaDonorId(gift.DonorId)).Returns(contactDonor);
            paymentService.Setup(mocked => mocked.CancelSubscription(contactDonor.ProcessorId, gift.SubscriptionId)).Returns(subscription);
            paymentService.Setup(mocked => mocked.CancelPlan(subscription.Plan.Id)).Returns(plan);
            mpDonorService.Setup(mocked => mocked.CancelRecurringGift(authUserToken, recurringGiftId));

            fixture.CancelRecurringGift(authUserToken, recurringGiftId);
            mpDonorService.VerifyAll();
            paymentService.VerifyAll();
        }
    }
}
