using crds_angular.Exceptions;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace crds_angular.Services
{
    public class StripeService : IPaymentService
    {
        private IRestClient stripeRestClient;

        private const string STRIPE_CUSTOMER_DESCRIPTION = "Crossroads Donor #{0}";

        public StripeService(IRestClient stripeRestClient)
        {
            this.stripeRestClient = stripeRestClient;
        }

        public string createCustomer(string token)
        {

            var request = new RestRequest("customers", Method.POST);
            request.AddParameter("description", string.Format(STRIPE_CUSTOMER_DESCRIPTION, "pending")); // adds to POST or URL querystring based on Method
            request.AddParameter("source", token);

            IRestResponse<StripeCustomer> response =
                (IRestResponse<StripeCustomer>)stripeRestClient.Execute<StripeCustomer>(request);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                Content content = JsonConvert.DeserializeObject<Content>(response.Content);
                throw new StripeException("Customer creation failed", content.error.type, content.error.message, content.error.code);
            }

            return response.Data.id;

        }

        public string updateCustomerDescription(string customer_token, int donor_id)
        {
            var request = new RestRequest("customers/" + customer_token, Method.POST);
            request.AddParameter("description", string.Format(STRIPE_CUSTOMER_DESCRIPTION, donor_id));

            IRestResponse<StripeCustomer> response =
                (IRestResponse<StripeCustomer>)stripeRestClient.Execute<StripeCustomer>(request);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                Content content = JsonConvert.DeserializeObject<Content>(response.Content);
                throw new StripeException("Customer update failed", content.error.type, content.error.message, content.error.code);
            }

            return (response.Data.id);
        }


        public string chargeCustomer(string customer_token, int amount, int donor_id)
        {

            var getCustomerRequest = new RestRequest("customers/" + customer_token, Method.GET);
            IRestResponse<StripeCustomer> getCustomerResponse =
                (IRestResponse<StripeCustomer>)stripeRestClient.Execute<StripeCustomer>(getCustomerRequest);
            if (getCustomerResponse.StatusCode == HttpStatusCode.BadRequest)
            {
                Content content = JsonConvert.DeserializeObject<Content>(getCustomerResponse.Content);
                throw new StripeException("Could not charge customer because customer lookup failed", content.error.type, content.error.message, content.error.code);
            }

            var chargeRequest = new RestRequest("charges", Method.POST);
            chargeRequest.AddParameter("amount", amount * 100);
            chargeRequest.AddParameter("currency", "usd");
            chargeRequest.AddParameter("source", getCustomerResponse.Data.default_source);
            chargeRequest.AddParameter("customer", getCustomerResponse.Data.id);
            chargeRequest.AddParameter("description", "Logged-in giver, donor_id# " + donor_id);

            IRestResponse<StripeCharge> chargeResponse =
                (IRestResponse<StripeCharge>)stripeRestClient.Execute<StripeCharge>(chargeRequest);
            if (chargeResponse.StatusCode == HttpStatusCode.BadRequest)
            {
                Content content = JsonConvert.DeserializeObject<Content>(chargeResponse.Content);
                throw new StripeException("Invalid charge request", content.error.type, content.error.message, content.error.code);
            }

            return chargeResponse.Data.id;
        }
    }

    public class Error
    {
        public string type { get; set; }
        public string message { get; set; }
        public string param { get; set; }
        public string code { get; set; }
    }

    public class Content
    {
        public Error error { get; set; }
    }
}

