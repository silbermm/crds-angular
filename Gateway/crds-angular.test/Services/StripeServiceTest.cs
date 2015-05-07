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
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private Mock<IRestClient> _restClientMock;
        private StripeService _fixture  ;

        [SetUp]
        public void Setup()
        {
           _configurationWrapper = new Mock<IConfigurationWrapper>();
           _restClientMock = new Mock<IRestClient>();
           _fixture = new StripeService(_restClientMock.Object);
        }

       [Test]
        public void shouldThrowExceptionWhenTokenIsInvalid()
        {
            var mockStripeResponse = new RestResponse<StripeService>();
            mockStripeResponse.StatusCode = HttpStatusCode.BadRequest;
            var request = new RestRequest("customers", Method.POST);
            request.AddParameter("description", "testing customers");
            request.AddParameter("source", It.IsAny<string>());
            _restClientMock.Setup(mock => mock.Execute(request)).Returns(mockStripeResponse);

            Assert.Throws<StripeException>(() => _fixture.createCustomer("tok_is_bad"));
        }

    }

  }
