using System.Net;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Services;
using RestSharp;

namespace crds_angular.Services
{
    public class StripeService : IPaymentService
    {
        private IRestClient stripeRestClient;

        public StripeService(IRestClient stripeRestClient)
        {
            this.stripeRestClient = stripeRestClient;
        }

        public string createCustomer(string token)
        {
            var request = new RestRequest("customers", Method.POST);
            request.AddParameter("description", "testing customers"); // adds to POST or URL querystring based on Method
            request.AddParameter("source", token);

            RestResponse<StripeCustomer> response = (RestResponse<StripeCustomer>)stripeRestClient.Execute<StripeCustomer>(request);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // TODO: deserialize content into StripeError and return message in StripeException
                throw new StripeException();
            } 

            return response.Data.id;
          
        }


        public string chargeCustomer(string customer_token, int amount, string donor_id)
        {
            var getCustomerRequest = new RestRequest("customers/"+customer_token, Method.GET);
            RestResponse<StripeCustomer> getCustomerResponse = (RestResponse<StripeCustomer>)stripeRestClient.Execute<StripeCustomer>(getCustomerRequest);
            if (getCustomerResponse.StatusCode == HttpStatusCode.BadRequest)
            {
                // TODO: deserialize content into StripeError and return message in StripeException
                throw new StripeException();
            } 

            var chargeRequest = new RestRequest("charges", Method.POST);
            chargeRequest.AddParameter("amount", amount * 100 );
            chargeRequest.AddParameter("currency", "usd");
            chargeRequest.AddParameter("source", getCustomerResponse.Data.default_source);
            chargeRequest.AddParameter("customer", getCustomerResponse.Data.id);
            chargeRequest.AddParameter("description", "Logged-in giver, donor_id# "+ donor_id);

            RestResponse<StripeCharge> chargeResponse = (RestResponse<StripeCharge>)stripeRestClient.Execute<StripeCharge>(chargeRequest);
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