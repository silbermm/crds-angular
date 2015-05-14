using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Results;

namespace crds_angular.test.controllers
{
    class DonorControllerTest
    {
        private DonorController fixture;
        private Mock<crds_angular.Services.Interfaces.IDonorService> donorService;
        private string authType;
        private string authToken;
        private static int contactId = 8675309;
        private static string processorId = "cus_test123456";
        private static string email = "automatedtest@crossroads.net";
        private static int donorId = 394256;
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
            donorService = new Mock<crds_angular.Services.Interfaces.IDonorService>();
            fixture = new DonorController(donorService.Object);

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

            donorService.Setup(mocked => mocked.GetDonorForAuthenticatedUser(It.IsAny<string>())).Returns((ContactDonor)null);
            donorService.Setup(mocked => mocked.CreateOrUpdateDonor(null, string.Empty, "tok_test", It.IsAny<DateTime>())).Returns(donor);

            IHttpActionResult result = fixture.Post(createDonorDto);
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<DonorDTO>), result);
            var okResult = (OkNegotiatedContentResult<DonorDTO>)result;
            Assert.AreEqual(donorId, okResult.Content.id);
            Assert.AreEqual(processorId, okResult.Content.Processor_ID);
        }

        [Test]
        public void TestGetSuccessGetDonorAuthenticated()
        {
            donorService.Setup(mocked => mocked.GetDonorForAuthenticatedUser(It.IsAny<string>())).Returns(donor);
            IHttpActionResult result = fixture.Get();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<DonorDTO>), result);
            var okResult = (OkNegotiatedContentResult<DonorDTO>)result;
            Assert.AreEqual(donorId, okResult.Content.id);
            Assert.AreEqual(processorId, okResult.Content.Processor_ID);
        }

        [Test]
        public void TestGetSuccessGetDonorUnauthenticated()
        {
            fixture.Request.Headers.Authorization = null;
            donorService.Setup(mocked => mocked.GetDonorForEmail(email)).Returns(donor);
            IHttpActionResult result = fixture.Get(email);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<DonorDTO>), result);
            var okResult = (OkNegotiatedContentResult<DonorDTO>)result;
            Assert.AreEqual(donorId, okResult.Content.id);
            Assert.AreEqual(processorId, okResult.Content.Processor_ID);
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

            donorService.Setup(mocked => mocked.GetDonorForEmail(createDonorDto.email_address)).Returns(lookupDonor);
            donorService.Setup(mocked => mocked.CreateOrUpdateDonor(It.Is<ContactDonor>(d => d == lookupDonor), createDonorDto.email_address, createDonorDto.stripe_token_id, It.IsAny<DateTime>())).Returns(createDonor);

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
            Assert.AreEqual(394256, responseDto.id);
            Assert.AreEqual("jenny_ive_got_your_number", responseDto.Processor_ID);
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

            donorService.Setup(mocked => mocked.GetDonorForEmail(createDonorDto.email_address)).Returns(lookupDonor);
            donorService.Setup(mocked => mocked.CreateOrUpdateDonor(It.Is<ContactDonor>(d => d == lookupDonor), createDonorDto.email_address, createDonorDto.stripe_token_id, It.IsAny<DateTime>())).Returns(createDonor);

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
            Assert.AreEqual(90210, responseDto.id);
            Assert.AreEqual("jenny_ive_got_your_number", responseDto.Processor_ID);
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

            donorService.Setup(mocked => mocked.GetDonorForEmail(createDonorDto.email_address)).Throws(lookupException);

            try {
                fixture.Post(createDonorDto);
                Assert.Fail("Expected exception was not thrown");
            } catch(Exception e) {
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

            donorService.Setup(mocked => mocked.GetDonorForEmail(createDonorDto.email_address)).Returns(lookupDonor);
            donorService.Setup(mocked => mocked.CreateOrUpdateDonor(It.Is<ContactDonor>(d => d == lookupDonor), createDonorDto.email_address, createDonorDto.stripe_token_id, It.IsAny<DateTime>())).Throws(createException);

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


    
    }
}
