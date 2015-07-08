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

        private const string StripeNetworkErrorResponseCode = "abort";

        private const int MaxChargesPerPage = 10;

        public StripeService(IRestClient stripeRestClient)
        {
            _stripeRestClient = stripeRestClient;
        }

        private static bool IsBadResponse(IRestResponse response)
        {
            return (response.ResponseStatus != ResponseStatus.Completed 
                    || response.StatusCode == HttpStatusCode.BadRequest
                    || response.StatusCode == HttpStatusCode.PaymentRequired);
        }

        private static void CheckStripeResponse(string errorMessage, IRestResponse response)
        {
            if (!IsBadResponse(response))
            {
                return;
            }

            var content = JsonConvert.DeserializeObject<Content>(response.Content);
            if (content == null || content.Error == null)
            {
                throw (new StripeException(HttpStatusCode.InternalServerError, errorMessage, StripeNetworkErrorResponseCode,
                    response.ErrorException.Message, null, null, null));
            }
            else
            {
                throw new StripeException(response.StatusCode, errorMessage, content.Error.Type, content.Error.Message, content.Error.Code, content.Error.DeclineCode, content.Error.Param);
            }
        }


        public string CreateCustomer(string customerToken)
        {
            var request = new RestRequest("customers", Method.POST);
            request.AddParameter("description", string.Format(StripeCustomerDescription, "pending")); // adds to POST or URL querystring based on Method
            request.AddParameter("source", customerToken);

            var response = _stripeRestClient.Execute<StripeCustomer>(request);
            CheckStripeResponse("Customer creation failed", response);

            return response.Data.id;

        }

        public SourceData UpdateCustomerSource(string customerToken, string cardToken)
        {
            var request = new RestRequest("customers/" + customerToken, Method.POST);
            request.AddParameter("source", cardToken);

            var response = _stripeRestClient.Execute<StripeCustomer>(request);
            CheckStripeResponse("Customer update to add source failed", response);

            var defaultSourceId = response.Data.default_source;
            var sources = response.Data.sources.data;
            var defaultSource = MapDefaultSource(sources, defaultSourceId);
            
            return defaultSource;

        }

        public string UpdateCustomerDescription(string customerToken, int donorId)
        {
            var request = new RestRequest("customers/" + customerToken, Method.POST);
            request.AddParameter("description", string.Format(StripeCustomerDescription, donorId));

            var response = _stripeRestClient.Execute<StripeCustomer>(request);
            CheckStripeResponse("Customer update failed", response);

            return (response.Data.id);
        }

        public SourceData GetDefaultSource(string customerToken)
        {
            var request = new RestRequest("customers/" + customerToken, Method.GET);

            var response = _stripeRestClient.Execute<StripeCustomer>(request);
            CheckStripeResponse("Could not get default source information because customer lookup failed", response);

            var defaultSourceId = response.Data.default_source;
            var sources = response.Data.sources.data;
            var defaultSource = MapDefaultSource(sources, defaultSourceId);

            return defaultSource;
        }

        private static SourceData MapDefaultSource(List<SourceData>sources, string defaultSourceId)
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

            var response = _stripeRestClient.Execute<StripeCharge>(request);
            CheckStripeResponse("Invalid charge request", response);

            return response.Data.Id;
        }

        public List<string> GetChargesForTransfer(string transferId)
        {
            var url = string.Format("transfers/{0}/transactions", transferId);
            var request = new RestRequest(url, Method.GET);
            request.AddParameter("count", MaxChargesPerPage);

            var charges = new List<string>();
            IRestResponse<StripeCharges> response;
            do
            {
                response = _stripeRestClient.Execute<StripeCharges>(request);
                CheckStripeResponse("Could not query transactions", response);

                charges.AddRange(response.Data.Data.Select(charge => charge.Id));

                request = new RestRequest(url, Method.GET);
                request.AddParameter("count", MaxChargesPerPage);
                request.AddParameter("starting_after", charges.Last());
            } while (response.Data.HasMore);

            return (charges);
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

