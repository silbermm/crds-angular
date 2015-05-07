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
using System.Collections.Generic;

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
            var stripeResponse = new Mock<IRestResponse<StripeCustomer>>(MockBehavior.Strict);
            stripeResponse.SetupGet(mocked => mocked.StatusCode).Returns(HttpStatusCode.BadRequest).Verifiable();

            restClient.Setup(mocked => mocked.Execute<StripeCustomer>(It.IsAny<IRestRequest>())).Returns(stripeResponse.Object);

            Assert.Throws<StripeException>(() => fixture.createCustomer("token"));

            restClient.Verify(mocked => mocked.Execute<StripeCustomer>(
                It.Is<RestRequest>(o =>
                    o.Method == Method.POST
                    && o.Resource.Equals("customers")
                    && parameterMatches("description", "testing customers", o.Parameters)
                    && parameterMatches("source", "token", o.Parameters)
                    )));
            restClient.VerifyAll();
            stripeResponse.VerifyAll();
        }

        [Test]
        public void shouldReturnSuccessfulCustomerId()
        {
            var customer = new StripeCustomer();
            customer.id = "12345";

            var stripeResponse = new Mock<IRestResponse<StripeCustomer>>(MockBehavior.Strict);
            stripeResponse.SetupGet(mocked => mocked.StatusCode).Returns(HttpStatusCode.OK).Verifiable();
            stripeResponse.SetupGet(mocked => mocked.Data).Returns(customer).Verifiable();

            restClient.Setup(mocked => mocked.Execute<StripeCustomer>(It.IsAny<IRestRequest>())).Returns(stripeResponse.Object);

            var response = fixture.createCustomer("token");
            restClient.Verify(mocked => mocked.Execute<StripeCustomer>(
                It.Is<IRestRequest>(o =>
                    o.Method == Method.POST
                    && o.Resource.Equals("customers")
                    && parameterMatches("description", "testing customers", o.Parameters)
                    && parameterMatches("source", "token", o.Parameters)
                    )));
            restClient.VerifyAll();
            stripeResponse.VerifyAll();

            Assert.AreEqual("12345", response);
        }

        [Test]
        public void shouldChargeCustomer()
        {
            var customer = new StripeCustomer();
            customer.id = "12345";
            customer.default_source = "some card";

            var getCustomerResponse = new Mock<IRestResponse<StripeCustomer>>(MockBehavior.Strict);
            getCustomerResponse.SetupGet(mocked => mocked.StatusCode).Returns(HttpStatusCode.OK).Verifiable();
            getCustomerResponse.SetupGet(mocked => mocked.Data).Returns(customer).Verifiable();

            restClient.Setup(mocked => mocked.Execute<StripeCustomer>(It.IsAny<IRestRequest>())).Returns(getCustomerResponse.Object);

            var charge = new StripeCharge();
            charge.id = "90210";

            var chargeResponse = new Mock<IRestResponse<StripeCharge>>(MockBehavior.Strict);
            chargeResponse.SetupGet(mocked => mocked.StatusCode).Returns(HttpStatusCode.OK).Verifiable();
            chargeResponse.SetupGet(mocked => mocked.Data).Returns(charge).Verifiable();

            restClient.Setup(mocked => mocked.Execute<StripeCharge>(It.IsAny<IRestRequest>())).Returns(chargeResponse.Object);

            var response = fixture.chargeCustomer("cust_token", 9090, "donor98765");

            restClient.Verify(mocked => mocked.Execute<StripeCustomer>(
                It.Is<IRestRequest>(o =>
                    o.Method == Method.GET
                    && o.Resource.Equals("customers/cust_token"))));

            restClient.Verify(mocked => mocked.Execute<StripeCharge>(
                It.Is<IRestRequest>(o =>
                    o.Method == Method.POST
                    && o.Resource.Equals("charges")
                    && parameterMatches("amount", 9090 * 100, o.Parameters)
                    && parameterMatches("currency", "usd", o.Parameters)
                    && parameterMatches("source", "some card", o.Parameters)
                    && parameterMatches("customer", "12345", o.Parameters)
                    && parameterMatches("description", "Logged-in giver, donor_id# donor98765", o.Parameters)
                    )));

            restClient.VerifyAll();
            getCustomerResponse.VerifyAll();
            chargeResponse.VerifyAll();

            Assert.AreEqual("90210", response);
        }

        private bool parameterMatches(string name, object value, List<Parameter> parms)
        {
            return(parms.Find(p => p.Name.Equals(name) && p.Value.Equals(value)) != null);
        }

    }

}
