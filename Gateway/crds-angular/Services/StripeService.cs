using System.Collections.Generic;
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

        public DefaultSourceDTO updateCustomerSource(string customerToken, string cardToken)
        {
            DefaultSourceDTO defaultSource = new DefaultSourceDTO();

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
            defaultSource = SetDefaultSource(sources, defaultSourceId);
            
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

        public DefaultSourceDTO getDefaultSource(string customer_token)
        {
            DefaultSourceDTO defaultSource = new DefaultSourceDTO();
         
            var getCustomerRequest = new RestRequest("customers/" + customer_token, Method.GET);

            IRestResponse<StripeCustomer> getCustomerResponse =
                (IRestResponse<StripeCustomer>)stripeRestClient.Execute<StripeCustomer>(getCustomerRequest);
            if (getCustomerResponse.StatusCode == HttpStatusCode.BadRequest)
            {
                Content content = JsonConvert.DeserializeObject<Content>(getCustomerResponse.Content);
                throw new StripeException("Could not get default source information because customer lookup failed", content.error.type, content.error.message, content.error.code);
            }
            var defaultSourceId = getCustomerResponse.Data.default_source;
            var sources = getCustomerResponse.Data.sources.data;
            defaultSource = SetDefaultSource(sources, defaultSourceId);

            return defaultSource;
        }

        public DefaultSourceDTO SetDefaultSource(List<SourceData>sources, string defaultSourceId )
        {
            DefaultSourceDTO defaultSource = new DefaultSourceDTO();

            foreach (var source in sources)
            {
                if (source.id == defaultSourceId)
                {
                   if (source.@object == "bank_account")
                    {
                        defaultSource.bank_account.routing = source.routing_number;
                        defaultSource.bank_account.last4 = source.last4;
                    }
                    else
                    {
                        defaultSource.credit_card.brand = source.brand;
                        defaultSource.credit_card.last4 = source.last4;
                        defaultSource.credit_card.name = source.name;
                        defaultSource.credit_card.address_zip = source.address_zip;
                        defaultSource.credit_card.exp_date = source.exp_month.PadLeft(2, '0') +
                                                             source.exp_year.Substring(2, 2);

                    }
                }
            }

            return defaultSource;
        }

        public string chargeCustomer(string customer_token, int amount, int donor_id, string pymt_type)
        {
            var chargeRequest = new RestRequest("charges", Method.POST);
            chargeRequest.AddParameter("amount", amount * 100);
            chargeRequest.AddParameter("currency", "usd");
            chargeRequest.AddParameter("customer", customer_token);
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

