using crds_angular.Controllers.API;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using crds_angular.Exceptions;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Models.Json;
using DonationStatus = crds_angular.Models.Crossroads.Stewardship.DonationStatus;

namespace crds_angular.test.controllers
{
    class DonorControllerTest
    {
        private DonorController fixture;
        private Mock<IDonorService> donorService;
        private Mock<IDonationService> _donationService;
        private Mock<IPaymentService> paymentService;
        private string authType;
        private string authToken;
        private static int contactId = 8675309;
        private static string processorId = "cus_test123456";
        private static string email = "automatedtest@crossroads.net";
        private static int donorId  = 394256;
        private static string last4 = "1234";
        private static string brand = "Visa";
        private static string address_zip = "45454";
        private ContactDonor donor = new ContactDonor()
        {
            DonorId = donorId,
            ProcessorId = processorId,
            ContactId = contactId,
            Email = email
        };

        [SetUp]
        public void SetUp()
        {
            donorService = new Mock<IDonorService>();
            _donationService = new Mock<IDonationService>();
            paymentService = new Mock<IPaymentService>();
            fixture = new DonorController(donorService.Object,paymentService.Object, _donationService.Object);

            authType = "auth_type";
            authToken = "auth_token";
            fixture.Request = new HttpRequestMessage();
            fixture.Request.Headers.Authorization = new AuthenticationHeaderValue(authType, authToken);
            fixture.RequestContext = new HttpRequestContext();

            // This is needed in order for Request.createResponse to work
            fixture.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            fixture.Request.SetConfiguration(new HttpConfiguration());
        }

        [Test]
        public void shouldPostToSuccessfullyCreateAuthenticatedDonor()
        {
            var createDonorDto = new CreateDonorDTO
            {
                stripe_token_id = "tok_test"
            };

            donorService.Setup(mocked => mocked.GetContactDonorForAuthenticatedUser(It.IsAny<string>())).Returns((ContactDonor)null);
            donorService.Setup(mocked => mocked.CreateOrUpdateContactDonor(null, string.Empty, string.Empty, "tok_test", It.IsAny<DateTime>())).Returns(donor);
            
            IHttpActionResult result = fixture.Post(createDonorDto);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<DonorDTO>), result);
            var okResult = (OkNegotiatedContentResult<DonorDTO>)result;
            Assert.AreEqual(donorId, okResult.Content.Id);
            Assert.AreEqual(processorId, okResult.Content.ProcessorId);
        }

        [Test]
        public void TestGetSuccessGetDonorAuthenticated()
        {
            var contactDonor = new ContactDonor
            {
                ContactId = 1,
                DonorId = 394256,
                ProcessorId = processorId
            };

            var default_source = new SourceData
            {
                last4 = "1234",
                brand = "Visa",
                address_zip = "45454"
            };
            
            donorService.Setup(mocked => mocked.GetContactDonorForAuthenticatedUser(It.IsAny<string>())).Returns(contactDonor);
            paymentService.Setup(mocked => mocked.GetDefaultSource(It.IsAny<string>())).Returns(default_source);
            IHttpActionResult result = fixture.Get();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<DonorDTO>), result);
            var okResult = (OkNegotiatedContentResult<DonorDTO>)result;
            Assert.AreEqual(donorId, okResult.Content.Id);
            Assert.AreEqual(processorId, okResult.Content.ProcessorId);
            Assert.AreEqual(brand, okResult.Content.DefaultSource.credit_card.brand);
            Assert.AreEqual(last4, okResult.Content.DefaultSource.credit_card.last4);
            Assert.AreEqual(address_zip, okResult.Content.DefaultSource.credit_card.address_zip);
        }

        [Test]
        public void TestGetGetDonorAuthenticatedNoPaymentProcessor()
        {
            var contactDonor = new ContactDonor
            {
                ContactId = 1,
                DonorId = 2,
                ProcessorId = null
            };
            donorService.Setup(mocked => mocked.GetContactDonorForAuthenticatedUser(It.IsAny<string>())).Returns(contactDonor);
            IHttpActionResult result = fixture.Get();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(NotFoundResult), result);
        }

        [Test]
        public void TestGetSuccessGetDonorUnauthenticated()
        {
            fixture.Request.Headers.Authorization = null;
            donorService.Setup(mocked => mocked.GetContactDonorForEmail(email)).Returns(donor);
            IHttpActionResult result = fixture.Get(email);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<DonorDTO>), result);
            var okResult = (OkNegotiatedContentResult<DonorDTO>)result;
            Assert.AreEqual(donorId, okResult.Content.Id);
            Assert.AreEqual(processorId, okResult.Content.ProcessorId);
        }

        [Test]
        public void TestGetGetDonorUnauthenticatedNoPaymentProcessor()
        {
            fixture.Request.Headers.Authorization = null;
            var contactDonor = new ContactDonor
            {
                ContactId = 1,
                DonorId = 2,
                ProcessorId = null
            };
            donorService.Setup(mocked => mocked.GetContactDonorForEmail(email)).Returns(contactDonor);
            IHttpActionResult result = fixture.Get(email);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(NotFoundResult), result);
        }

        [Test]
        public void shouldPostToSuccessfullyCreateGuestDonor()
        {
            fixture.Request.Headers.Authorization = null;

            var createDonorDto = new CreateDonorDTO
            {
                stripe_token_id = "tok_test",
                email_address = "me@here.com"
            };

            var lookupDonor = new ContactDonor
            {
                ContactId = 8675309,
                DonorId = 0,
                ProcessorId = null
            };

            var createDonor = new ContactDonor
            {
                ContactId = 8675309,
                DonorId = 394256,
                ProcessorId = "jenny_ive_got_your_number"
            };

            donorService.Setup(mocked => mocked.GetContactDonorForEmail(createDonorDto.email_address)).Returns(lookupDonor);
            donorService.Setup(mocked => mocked.CreateOrUpdateContactDonor(It.Is<ContactDonor>(d => d == lookupDonor), string.Empty, createDonorDto.email_address, createDonorDto.stripe_token_id, It.IsAny<DateTime>())).Returns(createDonor);

            IHttpActionResult result = fixture.Post(createDonorDto);

            donorService.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ResponseMessageResult), result);
            Assert.NotNull(((ResponseMessageResult)result).Response);
            Assert.AreEqual(HttpStatusCode.Created, ((ResponseMessageResult)result).Response.StatusCode);
            var content = ((ResponseMessageResult)result).Response.Content;
            Assert.NotNull(content);
            Assert.IsInstanceOf(typeof(ObjectContent<DonorDTO>), content);
            var responseDto = (DonorDTO)((ObjectContent)content).Value;
            Assert.AreEqual(394256, responseDto.Id);
            Assert.AreEqual("jenny_ive_got_your_number", responseDto.ProcessorId);
        }

        [Test]
        public void shouldPostToSuccessfullyReturnExistingGuestDonor()
        {
            fixture.Request.Headers.Authorization = null;

            var createDonorDto = new CreateDonorDTO
            {
                stripe_token_id = "tok_test",
                email_address = "me@here.com"
            };

            var lookupDonor = new ContactDonor
            {
                ContactId = 8675309,
                DonorId = 90210,
                ProcessorId = "jenny_ive_got_your_number"
            };

            var createDonor = new ContactDonor
            {
                ContactId = 8675309,
                DonorId = 90210,
                ProcessorId = "jenny_ive_got_your_number"
            };

            donorService.Setup(mocked => mocked.GetContactDonorForEmail(createDonorDto.email_address)).Returns(lookupDonor);
            donorService.Setup(mocked => mocked.CreateOrUpdateContactDonor(It.Is<ContactDonor>(d => d == lookupDonor), string.Empty, createDonorDto.email_address, createDonorDto.stripe_token_id, It.IsAny<DateTime>())).Returns(createDonor);

            IHttpActionResult result = fixture.Post(createDonorDto);

            donorService.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ResponseMessageResult), result);
            Assert.NotNull(((ResponseMessageResult)result).Response);
            Assert.AreEqual(HttpStatusCode.OK, ((ResponseMessageResult)result).Response.StatusCode);
            var content = ((ResponseMessageResult)result).Response.Content;
            Assert.NotNull(content);
            Assert.IsInstanceOf(typeof(ObjectContent<DonorDTO>), content);
            var responseDto = (DonorDTO)((ObjectContent)content).Value;
            Assert.AreEqual(90210, responseDto.Id);
            Assert.AreEqual("jenny_ive_got_your_number", responseDto.ProcessorId);
        }

        [Test]
        public void shouldThrowExceptionWhenDonorLookupFails()
        {
            fixture.Request.Headers.Authorization = null;

            var createDonorDto = new CreateDonorDTO
            {
                stripe_token_id = "tok_test",
                email_address = "me@here.com"
            };

            var lookupException = new Exception("Danger, Will Robinson!");

            donorService.Setup(mocked => mocked.GetContactDonorForEmail(createDonorDto.email_address)).Throws(lookupException);

            try
            {
                fixture.Post(createDonorDto);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(HttpResponseException), e.GetType());
            }

            donorService.VerifyAll();

        }

        [Test]
        public void shouldThrowExceptionWhenDonorCreationFails()
        {
            fixture.Request.Headers.Authorization = null;

            var createDonorDto = new CreateDonorDTO
            {
                stripe_token_id = "tok_test",
                email_address = "me@here.com"
            };

            var lookupDonor = new ContactDonor
            {
                ContactId = 8675309,
                DonorId = 90210,
                ProcessorId = "jenny_ive_got_your_number"
            };

            var createException = new Exception("Danger, Will Robinson!");

            donorService.Setup(mocked => mocked.GetContactDonorForEmail(createDonorDto.email_address)).Returns(lookupDonor);
            donorService.Setup(mocked => mocked.CreateOrUpdateContactDonor(It.Is<ContactDonor>(d => d == lookupDonor), string.Empty, createDonorDto.email_address, createDonorDto.stripe_token_id, It.IsAny<DateTime>())).Throws(createException);

            try
            {
                fixture.Post(createDonorDto);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(HttpResponseException), e.GetType());
            }

            donorService.VerifyAll();
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

            _donationService.Setup(mocked => mocked.GetDonationsForDonor(123, "1999", true)).Returns(dto);
            var response = fixture.GetDonations(123, "1999", true);
            _donationService.VerifyAll();

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<OkNegotiatedContentResult<DonationsDTO>>(response);
            var r = (OkNegotiatedContentResult<DonationsDTO>) response;
            Assert.IsNotNull(r.Content);
            Assert.AreSame(dto, r.Content);
        }

        [Test]
        public void TestGetDonationsNoDonationsFound()
        {
            _donationService.Setup(mocked => mocked.GetDonationsForDonor(123, "1999", true)).Returns((DonationsDTO) null);
            var response = fixture.GetDonations(123, "1999", true);
            _donationService.VerifyAll();

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<RestHttpActionResult<ApiErrorDto>>(response);
            var r = (RestHttpActionResult<ApiErrorDto>)response;
            Assert.IsNotNull(r.Content);
            Assert.AreEqual("No matching donations found", r.Content.Message);
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

            _donationService.Setup(mocked => mocked.GetDonationYearsForDonor(123)).Returns(dto);
            var response = fixture.GetDonationYears(123);
            _donationService.VerifyAll();

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<OkNegotiatedContentResult<DonationYearsDTO>>(response);
            var r = (OkNegotiatedContentResult<DonationYearsDTO>)response;
            Assert.IsNotNull(r.Content);
            Assert.AreSame(dto, r.Content);
        }

        [Test]
        public void TestGetDonationYearsNoYearsFound()
        {
            _donationService.Setup(mocked => mocked.GetDonationYearsForDonor(123)).Returns((DonationYearsDTO)null);
            var response = fixture.GetDonationYears(123);
            _donationService.VerifyAll();

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<RestHttpActionResult<ApiErrorDto>>(response);
            var r = (RestHttpActionResult<ApiErrorDto>)response;
            Assert.IsNotNull(r.Content);
            Assert.AreEqual("No donation years found", r.Content.Message);
        }

        [Test]
        public void TestUpdateDonorGuestDonor()
        {
            fixture.Request.Headers.Authorization = null;
            var dto = new UpdateDonorDTO
            {
                DonorId = "123",
                EmailAddress = "me@here.com",
                StripeTokenId = "456"
            };

            var contactDonor = new ContactDonor
            {
                DonorId = 123,
                ContactId = 789,
                Email = "me@here.com",
                ProcessorId = "102030",
                RegisteredUser = false,
            };

            var sourceData = new SourceData
            {
                bank_last4 = "5555",
                routing_number = "987654321"
            };

            donorService.Setup(mocked => mocked.GetContactDonorForEmail("me@here.com")).Returns(contactDonor);
            paymentService.Setup(mocked => mocked.UpdateCustomerSource(contactDonor.ProcessorId, dto.StripeTokenId))
                .Returns(sourceData);

            var result = fixture.UpdateDonor(dto);
            donorService.VerifyAll();
            paymentService.VerifyAll();

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkNegotiatedContentResult<DonorDTO>>(result);
            var donorDto = ((OkNegotiatedContentResult<DonorDTO>) result).Content;
            Assert.AreEqual(contactDonor.DonorId, donorDto.Id);
            Assert.AreEqual(contactDonor.ProcessorId, donorDto.ProcessorId);
            Assert.AreEqual(contactDonor.RegisteredUser, donorDto.RegisteredUser);

            Assert.IsNotNull(donorDto.DefaultSource.credit_card);
            Assert.IsNotNull(donorDto.DefaultSource.bank_account);
            Assert.AreEqual(sourceData.bank_last4, donorDto.DefaultSource.bank_account.last4);
            Assert.AreEqual(sourceData.routing_number, donorDto.DefaultSource.bank_account.routing);
        }

        [Test]
        public void TestUpdateDonorRegisteredDonor()
        {
            var dto = new UpdateDonorDTO
            {
                DonorId = "123",
                EmailAddress = "me@here.com",
                StripeTokenId = "456"
            };

            var contactDonor = new ContactDonor
            {
                DonorId = 123,
                ContactId = 789,
                Email = "me@here.com",
                ProcessorId = "102030",
                RegisteredUser = true,
            };

            var sourceData = new SourceData
            {
                brand = "Visa",
                last4 = "5432",
                address_zip = "90210",
                exp_month = "12",
                exp_year = "19"
            };

            donorService.Setup(mocked => mocked.GetContactDonorForAuthenticatedUser(It.IsAny<string>())).Returns(contactDonor);
            paymentService.Setup(mocked => mocked.UpdateCustomerSource(contactDonor.ProcessorId, dto.StripeTokenId))
                .Returns(sourceData);

            var result = fixture.UpdateDonor(dto);
            donorService.VerifyAll();
            paymentService.VerifyAll();

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkNegotiatedContentResult<DonorDTO>>(result);
            var donorDto = ((OkNegotiatedContentResult<DonorDTO>)result).Content;
            Assert.AreEqual(contactDonor.DonorId, donorDto.Id);
            Assert.AreEqual(contactDonor.ProcessorId, donorDto.ProcessorId);
            Assert.AreEqual(contactDonor.RegisteredUser, donorDto.RegisteredUser);
            Assert.IsNotNull(donorDto.DefaultSource.bank_account);
            Assert.IsNotNull(donorDto.DefaultSource.credit_card);
            Assert.AreEqual(sourceData.brand, donorDto.DefaultSource.credit_card.brand);
            Assert.AreEqual(sourceData.last4, donorDto.DefaultSource.credit_card.last4);
            Assert.AreEqual(sourceData.address_zip, donorDto.DefaultSource.credit_card.address_zip);
            Assert.AreEqual(sourceData.exp_month + sourceData.exp_year, donorDto.DefaultSource.credit_card.exp_date);
        }


        [Test]
        public void TestUpdateDonorLookupThrowsApplicationException()
        {
            fixture.Request.Headers.Authorization = null;
            var dto = new UpdateDonorDTO
            {
                DonorId = "123",
                EmailAddress = "me@here.com",
                StripeTokenId = "456"
            };

            var applicationException = new ApplicationException("whoa nelly");
            donorService.Setup(mocked => mocked.GetContactDonorForEmail("me@here.com")).Throws(applicationException);

            try
            {
                fixture.UpdateDonor(dto);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(HttpResponseException), e.GetType());
            }

            donorService.VerifyAll();
            paymentService.VerifyAll();
        }

        [Test]
        public void TestUpdateDonorStripeUpdateThrowsStripeException()
        {
            fixture.Request.Headers.Authorization = null;
            var dto = new UpdateDonorDTO
            {
                DonorId = "123",
                EmailAddress = "me@here.com",
                StripeTokenId = "456"
            };

            var contactDonor = new ContactDonor
            {
                DonorId = 123,
                ContactId = 789,
                Email = "me@here.com",
                ProcessorId = "102030",
                RegisteredUser = false,
            };

            var sourceData = new SourceData
            {
                bank_last4 = "5555",
                routing_number = "987654321"
            };

            donorService.Setup(mocked => mocked.GetContactDonorForEmail("me@here.com")).Returns(contactDonor);

            var stripeException = new PaymentProcessorException(HttpStatusCode.PaymentRequired, "auxMessage", "type", "message", "code", "decline", "param");
            paymentService.Setup(mocked => mocked.UpdateCustomerSource(contactDonor.ProcessorId, dto.StripeTokenId))
                .Throws(stripeException);

            var response = fixture.UpdateDonor(dto);
            Assert.AreEqual(typeof(RestHttpActionResult<PaymentProcessorErrorResponse>), response.GetType());
            var stripeErrorResponse = (RestHttpActionResult<PaymentProcessorErrorResponse>) response;
            var content = stripeErrorResponse.Content;
            Assert.AreEqual("type", content.Error.Type);
            Assert.AreEqual("message", content.Error.Message);
            Assert.AreEqual("code", content.Error.Code);
            Assert.AreEqual("decline", content.Error.DeclineCode);
            Assert.AreEqual("param", content.Error.Param);

            donorService.VerifyAll();
            paymentService.VerifyAll();
        }

        [Test]
        public void TestCreateDonorForUnauthenticatedUserStripeUpdateThrowsStripeException()
        {
            fixture.Request.Headers.Authorization = null;
            var dto = new CreateDonorDTO
            {
                email_address = "me@here.com",
                stripe_token_id = "456"
            };

            var contactDonor = new ContactDonor
            {
                DonorId = 123,
                ContactId = 789,
                Email = "me@here.com",
                ProcessorId = "102030",
                RegisteredUser = false,
            };

            var sourceData = new SourceData
            {
                bank_last4 = "5555",
                routing_number = "987654321"
            };

            var stripeException = new PaymentProcessorException(HttpStatusCode.PaymentRequired, "auxMessage", "type", "message", "code", "decline", "param");
            donorService.Setup(mocked => mocked.GetContactDonorForEmail("me@here.com")).Returns(contactDonor);
            donorService.Setup(
                (mocked => mocked.CreateOrUpdateContactDonor(contactDonor, string.Empty, "me@here.com", "456", It.IsAny<DateTime>())))
                .Throws(stripeException);

            var response = fixture.Post(dto);
            Assert.AreEqual(typeof(RestHttpActionResult<PaymentProcessorErrorResponse>), response.GetType());

            donorService.VerifyAll();
            paymentService.VerifyAll();
        }

        [Test]
        public void TestCreateDonorForAuthenticatedUserStripeUpdateThrowsStripeException()
        {
            var dto = new CreateDonorDTO
            {
                email_address = "me@here.com",
                stripe_token_id = "456"
            };

            var contactDonor = new ContactDonor
            {
                DonorId = 123,
                ContactId = 789,
                Email = "me@here.com",
                ProcessorId = "102030",
                RegisteredUser = true,
            };

            var sourceData = new SourceData
            {
                bank_last4 = "5555",
                routing_number = "987654321"
            };

            var stripeException = new PaymentProcessorException(HttpStatusCode.PaymentRequired, "auxMessage", "type", "message", "code", "decline", "param");
            donorService.Setup(mocked => mocked.GetContactDonorForAuthenticatedUser(It.IsAny<string>())).Returns(contactDonor);
            donorService.Setup(
                (mocked => mocked.CreateOrUpdateContactDonor(contactDonor, string.Empty, String.Empty, "456", It.IsAny<DateTime>())))
                .Throws(stripeException);

            var response = fixture.Post(dto);
            Assert.AreEqual(typeof(RestHttpActionResult<PaymentProcessorErrorResponse>), response.GetType());

            donorService.VerifyAll();
            paymentService.VerifyAll();
        }

    }
}
