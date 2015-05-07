using System;
using System.Configuration;
using System.Net;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using RestSharp;

namespace crds_angular.test.Services
{
    class StripeServiceTest
    {
        private Mock<IRestClient> restClient;
        private StripeService fixture;

        [SetUp]
        public void Setup()
        {
            restClient = new Mock<IRestClient>(MockBehavior.Strict);
            fixture = new StripeService(restClient.Object);
        }

        [Test]
        public void shouldThrowExceptionWhenTokenIsInvalid()
        {
            var stripeResponse = new Mock<IRestResponse<StripeCustomer>>();
            stripeResponse.SetupGet(mocked => mocked.StatusCode).Returns(HttpStatusCode.BadRequest);

            var request = new RestRequest("customers", Method.POST);
            request.AddParameter("description", "testing customers");
            request.AddParameter("source", "src");
            restClient.Setup(mocked => mocked.Execute<StripeCustomer>(It.IsAny<IRestRequest>())).Returns(stripeResponse.Object);

            Assert.Throws<StripeException>(() => fixture.createCustomer("tok_is_bad"));

            restClient.Verify(mocked => mocked.Execute<StripeCustomer>(
                It.Is<IRestRequest>(o =>
                    o.Parameters[0].Name.Equals("description")
                    && o.Parameters[1].Name.Equals("source")
                    && o.Parameters[1].Value.Equals("tok_is_bad"))));
        }

        [Test]
        public void shouldReturnSuccessfulCustomerId()
        {
            StripeCustomer customer = new StripeCustomer();
            customer.id = "12345";

            var stripeResponse = new Mock<IRestResponse<StripeCustomer>>();
            stripeResponse.SetupGet(mocked => mocked.StatusCode).Returns(HttpStatusCode.OK);
            stripeResponse.SetupGet(mocked => mocked.Data).Returns(customer);

            var request = new RestRequest("customers", Method.POST);
            request.AddParameter("description", "testing customers");
            request.AddParameter("source", "src");
            restClient.Setup(mocked => mocked.Execute<StripeCustomer>(It.IsAny<IRestRequest>())).Returns(stripeResponse.Object);

            var response = fixture.createCustomer("token");
            restClient.Verify(mocked => mocked.Execute<StripeCustomer>(
                It.Is<IRestRequest>(o =>
                    o.Parameters[0].Name.Equals("description")
                    && o.Parameters[1].Name.Equals("source")
                    && o.Parameters[1].Value.Equals("token"))));

            Assert.AreEqual("12345", response);
        }

    }

}
