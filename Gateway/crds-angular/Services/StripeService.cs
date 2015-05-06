using System.Net;
using crds_angular.Services.Interfaces;
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


        public string chargeCustomer(string customer_token, int amount, string donor_id)
        {
             _configurationWrapper = new ConfigurationWrapper();

            var client = new RestClient(_configurationWrapper.GetConfigValue("PaymentClient"))
            {
                Authenticator = new HttpBasicAuthenticator(_configurationWrapper.GetEnvironmentVarAsString("STRIPE_TEST_AUTH_TOKEN"), null)
            };
            var getCustomerRequest = new RestRequest("customer/"+customer_token, Method.GET);
            RestResponse<StripeCustomer> getCustomerResponse = (RestResponse<StripeCustomer>) client.Execute<StripeCustomer>(getCustomerRequest);
            if (getCustomerResponse.StatusCode == HttpStatusCode.BadRequest)
            {
                // TODO: deserialize content into StripeError and return message in StripeException
                throw new StripeException();
            } 

            var chargeRequest = new RestRequest("charges", Method.POST);
            chargeRequest.AddParameter("amount", amount * 100 );
            chargeRequest.AddParameter("currency", "usd");
            chargeRequest.AddParameter("source", getCustomerResponse.Data.default_source);
            chargeRequest.AddParameter("description", "Logged-in giver, donor_id# "+ donor_id);
 
           RestResponse<StripeCharge> chargeResponse = (RestResponse<StripeCharge>) client.Execute<StripeCharge>(getCustomerRequest);
            if (chargeResponse.StatusCode == HttpStatusCode.BadRequest)
            {
                // TODO: deserialize content into StripeError and return message in StripeException
                throw new StripeException();
            } 

            return chargeResponse.Data.id;
        }
    }

    public class StripeCharge
    {
        public string id { get; set; }

    }
}