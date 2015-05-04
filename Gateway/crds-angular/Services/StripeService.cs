using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using crds_angular.Models;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Services;
using RestSharp;

namespace crds_angular.Services
{
    public class StripeService : IPaymentService
    {
        private ConfigurationWrapper _configurationWrapper;

        public string createCustomer(string token)
        {
            _configurationWrapper = new ConfigurationWrapper();

            var client = new RestClient(_configurationWrapper.GetConfigValue("PaymentClient"))
            {
                Authenticator = new HttpBasicAuthenticator(_configurationWrapper.GetEnvironmentVarAsString("STRIPE_TEST_AUTH_TOKEN"), null)
            };
            
            var request = new RestRequest("customers", Method.POST);
            request.AddParameter("description", "testing customers"); // adds to POST or URL querystring based on Method
            request.AddParameter("source", token);
 
            RestResponse<StripeCustomer> response = (RestResponse<StripeCustomer>) client.Execute<StripeCustomer>(request);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // TODO: deserialize content into StripeError and return message in StripeException
                throw new StripeException();
            } 

            return response.Data.id;
          
        }
    }
}