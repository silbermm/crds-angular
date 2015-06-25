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
        private readonly IRestClient _stripeRestClient;

        private const string StripeCustomerDescription = "Crossroads Donor #{0}";

        public StripeService(IRestClient stripeRestClient)
        {
            _stripeRestClient = stripeRestClient;
        }

        private static bool IsBadResponse(IRestResponse response)
        {
            return (response.StatusCode == HttpStatusCode.BadRequest ||
                    response.StatusCode == HttpStatusCode.PaymentRequired);
        }

        public string CreateCustomer(string customerToken)
        {

            var request = new RestRequest("customers", Method.POST);
            request.AddParameter("description", string.Format(StripeCustomerDescription, "pending")); // adds to POST or URL querystring based on Method
            request.AddParameter("source", customerToken);

            var response = _stripeRestClient.Execute<StripeCustomer>(request);
            if (IsBadResponse(response))
            {
                Content content = JsonConvert.DeserializeObject<Content>(response.Content);
                throw new StripeException(response.StatusCode, "Customer creation failed", content.Error.Type, content.Error.Message, content.Error.Code, content.Error.DeclineCode, content.Error.Param);
            }

            return response.Data.id;

        }

        public SourceData UpdateCustomerSource(string customerToken, string cardToken)
        {
            var request = new RestRequest("customers/" + customerToken, Method.POST);
            request.AddParameter("source", cardToken);

            var response = _stripeRestClient.Execute<StripeCustomer>(request);
            if (IsBadResponse(response))
            {
                Content content = JsonConvert.DeserializeObject<Content>(response.Content);
                throw new StripeException(response.StatusCode, "Customer update to add source failed", content.Error.Type, content.Error.Message, content.Error.Code, content.Error.DeclineCode, content.Error.Param);
            }
            var defaultSourceId = response.Data.default_source;
            var sources = response.Data.sources.data;
            SourceData defaultSource = SetDefaultSource(sources, defaultSourceId);
            
            return defaultSource;

        }

        public string UpdateCustomerDescription(string customerToken, int donorId)
        {
            var request = new RestRequest("customers/" + customerToken, Method.POST);
            request.AddParameter("description", string.Format(StripeCustomerDescription, donorId));

            var response = _stripeRestClient.Execute<StripeCustomer>(request);
            if (IsBadResponse(response))
            {
                Content content = JsonConvert.DeserializeObject<Content>(response.Content);
                throw new StripeException(response.StatusCode, "Customer update failed", content.Error.Type, content.Error.Message, content.Error.Code, content.Error.DeclineCode, content.Error.Param);
            }

            return (response.Data.id);
        }

        public SourceData GetDefaultSource(string customerToken)
        {
            var request = new RestRequest("customers/" + customerToken, Method.GET);

            var response = _stripeRestClient.Execute<StripeCustomer>(request);
            if (IsBadResponse(response))
            {
                Content content = JsonConvert.DeserializeObject<Content>(response.Content);
                throw new StripeException(response.StatusCode, "Could not get default source information because customer lookup failed", content.Error.Type, content.Error.Message, content.Error.Code, content.Error.DeclineCode, content.Error.Param);
            }
            var defaultSourceId = response.Data.default_source;
            var sources = response.Data.sources.data;
            SourceData defaultSource = SetDefaultSource(sources, defaultSourceId);

            return defaultSource;
        }

        public SourceData SetDefaultSource(List<SourceData>sources, string defaultSourceId)
        {
            var defaultSource = new SourceData();

            foreach (var source in sources.Where(source => source.id == defaultSourceId))
            {
                if (source.@object == "bank_account")
                {
                    defaultSource.routing_number = source.routing_number;
                    defaultSource.bank_last4 = source.last4;
                }
                else
                {
                    defaultSource.brand = source.brand;
                    defaultSource.last4 = source.last4;
                    defaultSource.name = source.name;
                    defaultSource.address_zip = source.address_zip;
                    defaultSource.exp_month = source.exp_month.PadLeft(2, '0');
                    defaultSource.exp_year = source.exp_year.Substring(2, 2);
                }
            }

            return defaultSource;
        }

        public string ChargeCustomer(string customerToken, int amount, int donorId, string paymentType)
        {
            var request = new RestRequest("charges", Method.POST);
            request.AddParameter("amount", amount * 100);
            request.AddParameter("currency", "usd");
            request.AddParameter("customer", customerToken);
            request.AddParameter("description", "Donor ID #" + donorId);

            IRestResponse<StripeCharge> response = _stripeRestClient.Execute<StripeCharge>(request);
            if (IsBadResponse(response))
            {
                Content content = JsonConvert.DeserializeObject<Content>(response.Content);
                throw new StripeException(response.StatusCode, "Invalid charge request", content.Error.Type, content.Error.Message, content.Error.Code, content.Error.DeclineCode, content.Error.Param);
            }

            return response.Data.id;
        }
    }

    public class Error
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "param")]
        public string Param { get; set; }
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }
        [JsonProperty(PropertyName = "decline_code")]
        public string DeclineCode { get; set; }
    }

    public class Content
    {
        public Error Error { get; set; }
    }
}

