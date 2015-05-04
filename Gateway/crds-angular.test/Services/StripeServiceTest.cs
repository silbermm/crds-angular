using System;
using System.Configuration;
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
        private Mock<IPaymentService> _stripeServiceMock;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private StripeService _fixture  ;

        [SetUp]
        public void Setup()
        {
           _stripeServiceMock = new Mock<IPaymentService>();
           _configurationWrapper = new Mock<IConfigurationWrapper>();
           _fixture = new StripeService();
           var stripeCustomerId = "cus_crds1234";
        }

        [Test]
        public void shouldCallCreateCustomer()
        {
            var client = new RestClient(ConfigurationManager.AppSettings["PaymentClient"])
            {
                Authenticator =
                    new HttpBasicAuthenticator(
                        (_configurationWrapper.Setup(m => m.GetEnvironmentVarAsString("STRIPE_TEST_AUTH_TOKEN"))
                            .Returns("MockTestToken")).ToString(), null)
            };
            //need to fix this
            var stripeCustomer = _fixture.createCustomer(It.IsAny<string>());
            Assert.NotNull(stripeCustomer);
            StringAssert.StartsWith("cus_", stripeCustomer);
        }

        [Test]
        public void shouldThrowExceptionWhenTokenIsInvalid()
        {
           Assert.Throws<StripeException>(() => _fixture.createCustomer("tok_is_bad"));
        }

    }

  }
