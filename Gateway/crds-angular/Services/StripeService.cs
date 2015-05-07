using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Services;
using Newtonsoft.Json;
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
                Authenticator =
                    new HttpBasicAuthenticator(_configurationWrapper.GetEnvironmentVarAsString("STRIPE_AUTH_TOKEN"),
                        null)
            };

            var request = new RestRequest("customers", Method.POST);
            request.AddParameter("description", "testing customers"); // adds to POST or URL querystring based on Method
            request.AddParameter("source", token);

            RestResponse<StripeCustomer> response =
                (RestResponse<StripeCustomer>) client.Execute<StripeCustomer>(request);
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
                Authenticator =
                    new HttpBasicAuthenticator(_configurationWrapper.GetEnvironmentVarAsString("STRIPE_AUTH_TOKEN"),
                        null)
            };
            var getCustomerRequest = new RestRequest("customers/" + customer_token, Method.GET);
            RestResponse<StripeCustomer> getCustomerResponse =
                (RestResponse<StripeCustomer>) client.Execute<StripeCustomer>(getCustomerRequest);
            if (getCustomerResponse.StatusCode == HttpStatusCode.BadRequest)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Content content = serializer.Deserialize<Content>(getCustomerResponse.Content);
                throw new StripeException(content.error.message);
            }

            var chargeRequest = new RestRequest("charges", Method.POST);
            chargeRequest.AddParameter("amount", amount*100);
            chargeRequest.AddParameter("currency", "usd");
            chargeRequest.AddParameter("source", getCustomerResponse.Data.default_source);
            chargeRequest.AddParameter("customer", getCustomerResponse.Data.id);
            chargeRequest.AddParameter("description", "Logged-in giver, donor_id# " + donor_id);

            RestResponse<StripeCharge> chargeResponse =
                (RestResponse<StripeCharge>) client.Execute<StripeCharge>(chargeRequest);
            if (chargeResponse.StatusCode == HttpStatusCode.BadRequest)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Content content = serializer.Deserialize<Content>(chargeResponse.Content);
                throw new StripeException(content.error.message);
            }

            return chargeResponse.Data.id;
        }
    }

    public class StripeCharge
    {
        public string id { get; set; }

    }

    public class Error
    {
        public string type { get; set; }
        public string message { get; set; }
        public string param { get; set; }
    }

    public class Content
    {
        public Error error { get; set; }
    }
}

