using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Services;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Services;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace crds_angular.test.Services
{
    internal class PaymentServiceTest
    {
     public Mock<IConfigurationWrapper> _configurationWrapper;

        [Test]
        public void TestCreateCustomer()
        {
            _configurationWrapper = new Mock<IConfigurationWrapper>();
          
            var client = new RestClient(ConfigurationManager.AppSettings["PaymentClient"])
            {
                Authenticator =
                    new HttpBasicAuthenticator(
                        (_configurationWrapper.Setup(m => m.GetEnvironmentVarAsString("STRIPE_TEST_AUTH_TOKEN"))
                            .Returns("MockTestToken")).ToString(), null)
            };

        }

    }
}
