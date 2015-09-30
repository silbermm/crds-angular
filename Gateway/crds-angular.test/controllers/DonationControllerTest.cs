using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Exceptions;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Models.Json;
using MinistryPlatform.Translation.Services.Interfaces;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using Moq;
using NUnit.Framework;
using DonationStatus = crds_angular.Models.Crossroads.Stewardship.DonationStatus;
using IDonorService = crds_angular.Services.Interfaces.IDonorService;
using IDonationService = crds_angular.Services.Interfaces.IDonationService;

namespace crds_angular.test.controllers
{
    class DonationControllerTest
    {
        private DonationController fixture;
        private Mock<MinistryPlatform.Translation.Services.Interfaces.IDonorService> donorServiceMock;
        private Mock<IPaymentService> stripeServiceMock;
        private Mock<IAuthenticationService> authenticationServiceMock;
        private Mock<IDonorService> gatewayDonorServiceMock;
        private Mock<IDonationService> gatewayDonationServiceMock;
        private Mock<IUserImpersonationService> impersonationService;
        private Mock<MinistryPlatform.Translation.Services.Interfaces.IDonationService> mpDonationService; 
        private Mock<IPledgeService> mpPledgeService;
        private string authToken;
        private string authType;

        [SetUp]
        public void SetUp()
        {
            donorServiceMock = new Mock<MinistryPlatform.Translation.Services.Interfaces.IDonorService>();
            gatewayDonorServiceMock = new Mock<IDonorService>();
            stripeServiceMock = new Mock<IPaymentService>();
            authenticationServiceMock = new Mock<IAuthenticationService>();
            gatewayDonationServiceMock = new Mock<IDonationService>();
            mpPledgeService = new Mock<IPledgeService>();
            impersonationService = new Mock<IUserImpersonationService>();
            mpDonationService = new Mock<MinistryPlatform.Translation.Services.Interfaces.IDonationService>();

            fixture = new DonationController(donorServiceMock.Object, stripeServiceMock.Object,
                authenticationServiceMock.Object, gatewayDonorServiceMock.Object, gatewayDonationServiceMock.Object, mpDonationService.Object, mpPledgeService.Object, impersonationService.Object);

            authType = "auth_type";
            authToken = "auth_token";
            fixture.Request = new HttpRequestMessage();
            fixture.Request.Headers.Authorization = new AuthenticationHeaderValue(authType, authToken);
            fixture.RequestContext = new HttpRequestContext();
        }

        [Test]
        public void TestGetDonations()
        {
            var donations = new List<DonationDTO>
            {
                new DonationDTO
                {
                    BatchId = 123,
                    Amount = 78900,
                    DonationDate = DateTime.Now,
                    Id = "456",
                    Source = new DonationSourceDTO
                    {
                    SourceType = PaymentType.CreditCard,
                    CardType = CreditCardType.AmericanExpress,
                    Name = "ending in 1234",
                    PaymentProcessorId = "tx_123",
                    },
                    Email = "me@here.com",
                    ProgramId = "3",
                    Status = DonationStatus.Succeeded
                }
            };
            var dto = new DonationsDTO();
            dto.Donations.AddRange(donations);

            gatewayDonationServiceMock.Setup(mocked => mocked.GetDonationsForAuthenticatedUser(authType + " " + authToken, "1999", null, true)).Returns(dto);
            var response = fixture.GetDonations("1999", null, true);
            gatewayDonationServiceMock.VerifyAll();

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<OkNegotiatedContentResult<DonationsDTO>>(response);
            var r = (OkNegotiatedContentResult<DonationsDTO>)response;
            Assert.IsNotNull(r.Content);
            Assert.AreSame(dto, r.Content);
        }

        [Test]
        public void TestGetDonationsNoDonationsFound()
        {
            gatewayDonationServiceMock.Setup(mocked => mocked.GetDonationsForAuthenticatedUser(authType + " " + authToken, "1999", null, true)).Returns((DonationsDTO)null);
            var response = fixture.GetDonations("1999", null, true);
            gatewayDonationServiceMock.VerifyAll();

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<RestHttpActionResult<ApiErrorDto>>(response);
            var r = (RestHttpActionResult<ApiErrorDto>)response;
            Assert.IsNotNull(r.Content);
            Assert.AreEqual("No matching donations found", r.Content.Message);
        }

        [Test]
        public void TestGetDonationsImpersonationNotAllowed()
        {
            donorServiceMock.Setup(mocked => mocked.GetEmailViaDonorId(123)).Returns(new ContactDonor
            {
                Email = "me@here.com"
            });
            impersonationService.Setup(mocked => mocked.WithImpersonation(authType + " " + authToken, "me@here.com", It.IsAny<Func<DonationsDTO>>()))
                .Throws<ImpersonationNotAllowedException>();
            var response = fixture.GetDonations("1999", null, true, 123);
            gatewayDonationServiceMock.VerifyAll();

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<RestHttpActionResult<ApiErrorDto>>(response);
            var r = (RestHttpActionResult<ApiErrorDto>)response;
            Assert.AreEqual(HttpStatusCode.Forbidden, r.StatusCode);
            Assert.IsNotNull(r.Content);
            Assert.AreEqual("User is not authorized to impersonate other users.", r.Content.Message);
        }

        [Test]
        public void TestGetDonationsImpersonationUserNotFound()
        {
            donorServiceMock.Setup(mocked => mocked.GetEmailViaDonorId(123)).Returns(new ContactDonor
            {
                Email = "me@here.com"
            });
            impersonationService.Setup(mocked => mocked.WithImpersonation(authType + " " + authToken, "me@here.com", It.IsAny<Func<DonationsDTO>>()))
                .Throws(new ImpersonationUserNotFoundException("me@here.com"));
            var response = fixture.GetDonations("1999", null, true, 123);
            gatewayDonationServiceMock.VerifyAll();

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<RestHttpActionResult<ApiErrorDto>>(response);
            var r = (RestHttpActionResult<ApiErrorDto>)response;
            Assert.AreEqual(HttpStatusCode.Conflict, r.StatusCode);
            Assert.IsNotNull(r.Content);
            Assert.AreEqual("Could not locate user 'me@here.com' to impersonate", r.Content.Message);
        }

        [Test]
        public void TestGetDonationYears()
        {
            var donationYears = new List<string>
            {
                "1999",
                "2010",
                "2038"
            };
            var dto = new DonationYearsDTO();
            dto.AvailableDonationYears.AddRange(donationYears);

            gatewayDonationServiceMock.Setup(mocked => mocked.GetDonationYearsForAuthenticatedUser(authType + " " + authToken)).Returns(dto);
            var response = fixture.GetDonationYears();
            gatewayDonationServiceMock.VerifyAll();

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<OkNegotiatedContentResult<DonationYearsDTO>>(response);
            var r = (OkNegotiatedContentResult<DonationYearsDTO>)response;
            Assert.IsNotNull(r.Content);
            Assert.AreSame(dto, r.Content);
        }

        [Test]
        public void TestGetDonationYearsNoYearsFound()
        {
            gatewayDonationServiceMock.Setup(mocked => mocked.GetDonationYearsForAuthenticatedUser(authType + " " + authToken)).Returns((DonationYearsDTO)null);
            var response = fixture.GetDonationYears();
            gatewayDonationServiceMock.VerifyAll();

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<RestHttpActionResult<ApiErrorDto>>(response);
            var r = (RestHttpActionResult<ApiErrorDto>)response;
            Assert.IsNotNull(r.Content);
            Assert.AreEqual("No donation years found", r.Content.Message);
        }


        [Test]
        public void testPostToCreateDonationAndDistributionAuthenticated()
        {
            var contactId = 999999;
            var donationId = 6186818;
            var charge = new StripeCharge()
            {
                Id = "ch_crdscharge86868",
                BalanceTransaction = new StripeBalanceTransaction()
                {
                    Fee = 987
                }
            };

            var createDonationDTO = new CreateDonationDTO
            {
                ProgramId = "3", //crossroads
                Amount = 86868,
                DonorId = 394256,
                EmailAddress = "test@test.com"
            };

            var donor = new ContactDonor
            {
                ContactId = contactId,
                DonorId = 424242,
                SetupDate = new DateTime(),
                StatementFreq = "1",
                StatementMethod = "2",
                StatementType = "3",
                ProcessorId = "cus_test1234567",
                Email = "moc.tset@tset"
            };

            authenticationServiceMock.Setup(mocked => mocked.GetContactId(authType + " " + authToken)).Returns(contactId);

            donorServiceMock.Setup(mocked => mocked.GetContactDonor(contactId))
                .Returns(donor);

            stripeServiceMock.Setup(
                mocked => mocked.ChargeCustomer(donor.ProcessorId, createDonationDTO.Amount, donor.DonorId))
                .Returns(charge);

            donorServiceMock.Setup(mocked => mocked.
                CreateDonationAndDistributionRecord(createDonationDTO.Amount, charge.BalanceTransaction.Fee, donor.DonorId,
                    createDonationDTO.ProgramId, null, charge.Id, createDonationDTO.PaymentType, donor.ProcessorId, It.IsAny<DateTime>(), true, null))
                    .Returns(donationId);

            IHttpActionResult result = fixture.Post(createDonationDTO);

            authenticationServiceMock.VerifyAll();
            donorServiceMock.VerifyAll();
            stripeServiceMock.VerifyAll();
            donorServiceMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<DonationDTO>), result);
            var okResult = (OkNegotiatedContentResult<DonationDTO>)result;
            Assert.AreEqual(6186818, donationId);

            var resultDto = ((OkNegotiatedContentResult<DonationDTO>) result).Content;
            Assert.IsNotNull(resultDto);
            Assert.AreEqual(donor.Email, resultDto.Email);
        }

        [Test]
        public void testPostToCreateDonationAndDistributionWithPledgeAuthenticated()
        {
            var contactId = 999999;
            var donationId = 6186818;
            var charge = new StripeCharge()
            {
                Id = "ch_crdscharge86868",
                BalanceTransaction = new StripeBalanceTransaction()
                {
                    Fee = 987
                }
            };

            var createDonationDTO = new CreateDonationDTO
            {
                ProgramId = "3", //crossroads
                Amount = 86868,
                DonorId = 394256,
                EmailAddress = "test@test.com",
                PledgeCampaignId = 23,
                PledgeDonorId = 42,
                GiftMessage = "Don't look a Gift Horse in the Mouth!"
            };

            var donor = new ContactDonor
            {
                ContactId = contactId,
                DonorId = 424242,
                SetupDate = new DateTime(),
                StatementFreq = "1",
                StatementMethod = "2",
                StatementType = "3",
                ProcessorId = "cus_test1234567",
                Email = "moc.tset@tset"
            };

            var pledgeId = 3456;

            authenticationServiceMock.Setup(mocked => mocked.GetContactId(authType + " " + authToken)).Returns(contactId);

            donorServiceMock.Setup(mocked => mocked.GetContactDonor(contactId))
                .Returns(donor);

            mpPledgeService.Setup(mocked => mocked.GetPledgeByCampaignAndDonor(createDonationDTO.PledgeCampaignId.Value, createDonationDTO.PledgeDonorId.Value)).Returns(pledgeId);

            mpDonationService.Setup(mocked => mocked.SendMessageFromDonor(pledgeId, createDonationDTO.GiftMessage));

            stripeServiceMock.Setup(
                mocked => mocked.ChargeCustomer(donor.ProcessorId, createDonationDTO.Amount, donor.DonorId))
                .Returns(charge);

            donorServiceMock.Setup(mocked => mocked.
                CreateDonationAndDistributionRecord(createDonationDTO.Amount, charge.BalanceTransaction.Fee, donor.DonorId,
                    createDonationDTO.ProgramId, pledgeId, charge.Id, createDonationDTO.PaymentType, donor.ProcessorId, It.IsAny<DateTime>(), true, null))
                    .Returns(donationId);

            IHttpActionResult result = fixture.Post(createDonationDTO);

            authenticationServiceMock.VerifyAll();
            donorServiceMock.VerifyAll();
            stripeServiceMock.VerifyAll();
            donorServiceMock.VerifyAll();
            mpPledgeService.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<DonationDTO>), result);
            var okResult = (OkNegotiatedContentResult<DonationDTO>)result;
            Assert.AreEqual(6186818, donationId);

            var resultDto = ((OkNegotiatedContentResult<DonationDTO>)result).Content;
            Assert.IsNotNull(resultDto);
            Assert.AreEqual(donor.Email, resultDto.Email);
        }

        [Test]
        public void testPostToCreateDonationAndDistributionWithPledgeUnauthenticated()
        {
            var contactId = 999999;
            var donationId = 6186818;
            var charge = new StripeCharge()
            {
                Id = "ch_crdscharge86868",
                BalanceTransaction = new StripeBalanceTransaction()
                {
                    Fee = 987
                }
            };

            var createDonationDTO = new CreateDonationDTO
            {
                ProgramId = "3", //crossroads
                Amount = 86868,
                DonorId = 394256,
                EmailAddress = "test@test.com",
                PledgeCampaignId = 23,
                PledgeDonorId = 42,
                GiftMessage = "Don't look a Gift Horse in the Mouth!"
            };

            var donor = new ContactDonor
            {
                ContactId = contactId,
                DonorId = 424242,
                SetupDate = new DateTime(),
                StatementFreq = "1",
                StatementMethod = "2",
                StatementType = "3",
                ProcessorId = "cus_test1234567",
                Email = "moc.tset@tset"
            };

            var pledgeId = 3456;

            fixture.Request.Headers.Authorization = null;
            gatewayDonorServiceMock.Setup(mocked => mocked.GetContactDonorForEmail(createDonationDTO.EmailAddress)).Returns(donor);

            mpPledgeService.Setup(mocked => mocked.GetPledgeByCampaignAndDonor(createDonationDTO.PledgeCampaignId.Value, createDonationDTO.PledgeDonorId.Value)).Returns(pledgeId);

            mpDonationService.Setup(mocked => mocked.SendMessageFromDonor(pledgeId, createDonationDTO.GiftMessage));

            stripeServiceMock.Setup(mocked => mocked.ChargeCustomer(donor.ProcessorId, createDonationDTO.Amount, donor.DonorId)).
                Returns(charge);


            donorServiceMock.Setup(mocked => mocked.
                CreateDonationAndDistributionRecord(createDonationDTO.Amount, charge.BalanceTransaction.Fee, donor.DonorId,
                    createDonationDTO.ProgramId, pledgeId, charge.Id, createDonationDTO.PaymentType, donor.ProcessorId, It.IsAny<DateTime>(), false, null))
                    .Returns(donationId);


            IHttpActionResult result = fixture.Post(createDonationDTO);

           
            donorServiceMock.VerifyAll();
            stripeServiceMock.VerifyAll();
            donorServiceMock.VerifyAll();
            mpPledgeService.VerifyAll();

            donorServiceMock.VerifyAll();
            stripeServiceMock.VerifyAll();
            donorServiceMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<DonationDTO>), result);
            var okResult = (OkNegotiatedContentResult<DonationDTO>)result;
            Assert.AreEqual(6186818, donationId);

            var resultDto = ((OkNegotiatedContentResult<DonationDTO>)result).Content;
            Assert.IsNotNull(resultDto);
            Assert.AreEqual(donor.Email, resultDto.Email);
        }

        [Test]
        public void testPostToCreateDonationAndDistributionUnauthenticated()
        {
            var contactId = 999999;
            var donationId = 6186818;
            var charge = new StripeCharge()
            {
                Id = "ch_crdscharge86868",
                BalanceTransaction = new StripeBalanceTransaction()
                {
                    Fee = 987
                }
            };
           
            var createDonationDTO = new CreateDonationDTO
            {
                ProgramId = "3", //crossroads
                Amount = 86868,
                DonorId = 394256,
                EmailAddress = "test@test.com"
            };

            var donor = new ContactDonor
            {
                ContactId = contactId,
                DonorId = 424242,
                SetupDate = new DateTime(),
                StatementFreq = "1",
                StatementMethod = "2",
                StatementType = "3",
                ProcessorId = "cus_test1234567",
                Email = "moc.tset@tset"
            };

            fixture.Request.Headers.Authorization = null;
            gatewayDonorServiceMock.Setup(mocked => mocked.GetContactDonorForEmail(createDonationDTO.EmailAddress)).Returns(donor);
            
            stripeServiceMock.Setup(mocked => mocked.ChargeCustomer(donor.ProcessorId, createDonationDTO.Amount, donor.DonorId)).
                Returns(charge);

            donorServiceMock.Setup(mocked => mocked.
                CreateDonationAndDistributionRecord(createDonationDTO.Amount, charge.BalanceTransaction.Fee, donor.DonorId,
                    createDonationDTO.ProgramId, null, charge.Id, createDonationDTO.PaymentType, donor.ProcessorId, It.IsAny<DateTime>(), false, null))
                    .Returns(donationId);

            IHttpActionResult result = fixture.Post(createDonationDTO);

            donorServiceMock.VerifyAll();
            stripeServiceMock.VerifyAll();
            donorServiceMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<DonationDTO>), result);
            var okResult = (OkNegotiatedContentResult<DonationDTO>)result;
            Assert.AreEqual(6186818, donationId);

            var resultDto = ((OkNegotiatedContentResult<DonationDTO>)result).Content;
            Assert.IsNotNull(resultDto);
            Assert.AreEqual(donor.Email, resultDto.Email);
        }

    }
}
