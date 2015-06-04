using System.Linq;
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

        public SourceData updateCustomerSource(string customerToken, string cardToken)
        {
            SourceData defaultSource = new SourceData();

            var request = new RestRequest("customers/" + customerToken, Method.POST);
            request.AddParameter("source", cardToken);

            IRestResponse<StripeCustomer> response =
                (IRestResponse<StripeCustomer>)stripeRestClient.Execute<StripeCustomer>(request);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                Content content = JsonConvert.DeserializeObject<Content>(response.Content);
                throw new StripeException("Customer update to add source failed", content.error.type, content.error.message, content.error.code);
            }
            var defaultSourceId = response.Data.default_source;
            var sources = response.Data.sources.data;
            foreach (var source in sources)
            {
                if (source.id == defaultSourceId)
                {
                    defaultSource.brand = source.brand;
                    defaultSource.last4 = source.last4;
                    defaultSource.name = source.name;
                    defaultSource.address_zip = source.address_zip;
                    defaultSource.exp_month = source.exp_month;
                    defaultSource.exp_year = source.exp_year.Substring(2,2);
                }
            }

            return defaultSource;

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

        public SourceData getDefaultSource(string customer_token)
        {
            SourceData default_source = new SourceData();
            
            var getCustomerRequest = new RestRequest("customers/" + customer_token, Method.GET);

            IRestResponse<StripeCustomer> getCustomerResponse =
                (IRestResponse<StripeCustomer>)stripeRestClient.Execute<StripeCustomer>(getCustomerRequest);
            if (getCustomerResponse.StatusCode == HttpStatusCode.BadRequest)
            {
                Content content = JsonConvert.DeserializeObject<Content>(getCustomerResponse.Content);
                throw new StripeException("Could not get default source information because customer lookup failed", content.error.type, content.error.message, content.error.code);
            }
            var default_source_id = getCustomerResponse.Data.default_source;
            var sources = getCustomerResponse.Data.sources.data;
            foreach (var source in sources)
            {
                if (source.id == default_source_id)
                {
                    default_source.brand = source.brand;
                    default_source.last4 = source.last4;
                    default_source.name = source.name;
                    default_source.address_zip = source.address_zip;
                    default_source.exp_month = source.exp_month.PadLeft(2,'0');
                    default_source.exp_year = source.exp_year.Substring(2,2);
                }   
            }
            
            return default_source;
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
            chargeRequest.AddParameter("description", "Donor ID #" + donor_id);

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

