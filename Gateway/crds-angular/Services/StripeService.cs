using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Exceptions;
using crds_angular.Services.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using crds_angular.Models.Crossroads.Stewardship;
using Crossroads.Utilities;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using RestSharp.Extensions;

namespace crds_angular.Services
{
    public class StripeService : IPaymentService
    {
        private readonly IRestClient _stripeRestClient;

        private const string StripeCustomerDescription = "Crossroads Donor #{0}";

        private const string StripeNetworkErrorResponseCode = "abort";

        private readonly int _maxQueryResultsPerPage;

        private readonly IContentBlockService _contentBlockService;

        public StripeService(IRestClient stripeRestClient, IConfigurationWrapper configuration, IContentBlockService contentBlockService)
        {
            _stripeRestClient = stripeRestClient;
            _maxQueryResultsPerPage = configuration.GetConfigIntValue("MaxStripeQueryResultsPerPage");
            _contentBlockService = contentBlockService;
        }

        private static bool IsBadResponse(IRestResponse response)
        {
            return (response.ResponseStatus != ResponseStatus.Completed 
                    || response.StatusCode == HttpStatusCode.BadRequest
                    || response.StatusCode == HttpStatusCode.PaymentRequired);
        }

        private void CheckStripeResponse(string errorMessage, IRestResponse response)
        {
            if (!IsBadResponse(response))
            {
                return;
            }

            var content = JsonConvert.DeserializeObject<Content>(response.Content);
            if (content == null || content.Error == null)
            {
                throw(AddGlobalErrorMessage(new PaymentProcessorException(HttpStatusCode.InternalServerError, errorMessage, StripeNetworkErrorResponseCode,
                    response.ErrorException.Message, null, null, null)));
            }
            else
            {
                throw(AddGlobalErrorMessage(new PaymentProcessorException(response.StatusCode, errorMessage, content.Error.Type, content.Error.Message, content.Error.Code, content.Error.DeclineCode, content.Error.Param)));
            }
        }

        private PaymentProcessorException AddGlobalErrorMessage(PaymentProcessorException e)
        {
            // This same logic exists on the Angular side in app/give/services/payment_service.js.
            // This is because of the Stripe "tokens" call, which goes directly to Stripe, not via our API.  We
            // are implementing the same here in the interest of keeping our application somewhat agnostic to
            // the underlying payment processor.
            if ("abort".Equals(e.Type) || "abort".Equals(e.Code))
            {
                e.GlobalMessage = _contentBlockService["paymentMethodProcessingError"];
            }
            else if ("card_error".Equals(e.Type))
            {
                if (e.Code != null && ("card_declined".Equals(e.Code) || e.Code.Matches("^incorrect") || e.Code.Matches("^invalid")))
                {
                    e.GlobalMessage = _contentBlockService["paymentMethodDeclined"];
                }
                else if ("processing_error".Equals(e.Code))
                {
                    e.GlobalMessage = _contentBlockService["paymentMethodProcessingError"];
                }
            }
            else if ("bank_account".Equals(e.Param))
            {
                if ("invalid_request_error".Equals(e.Type))
                {
                    e.GlobalMessage = _contentBlockService["paymentMethodDeclined"];
                }
            }
            else
            {
                e.GlobalMessage = _contentBlockService["failedResponse"];
            }
            return (e);
        }

        public StripeCustomer CreateCustomer(string customerToken)
        {
            var request = new RestRequest("customers", Method.POST);
            request.AddParameter("description", string.Format(StripeCustomerDescription, "pending")); // adds to POST or URL querystring based on Method
            request.AddParameter("source", customerToken);

            var response = _stripeRestClient.Execute<StripeCustomer>(request);
            CheckStripeResponse("Customer creation failed", response);
            
            return response.Data;
        }

        public string CreateToken(string accountNumber, string routingNumber)
        {
            var request = new RestRequest("tokens", Method.POST);
            request.AddParameter("bank_account[account_number]", accountNumber);
            request.AddParameter("bank_account[routing_number]", routingNumber);
            request.AddParameter("bank_account[country]", "US");
            request.AddParameter("bank_account[currency]", "USD");
            // TODO Should be able to use request.AddJsonBody here, but that seems to ignore the property annotations
            //request.RequestFormat = DataFormat.Json;
            //request.AddJsonBody(new StripeBankAccount
            //{
            //    AccountNumber = accountNumber,
            //    RoutingNumber = routingNumber,
            //    Country = "US",
            //    Currency = "USD"
            //});

            var response = _stripeRestClient.Execute<StripeToken>(request);
            CheckStripeResponse("Token creation failed", response);

            return (response.Data.Id);
        }

        public SourceData UpdateCustomerSource(string customerToken, string cardToken)
        {
            //Passing source will create a new source object, make it the new customer default source, and delete the old customer default if one exist
            var request = new RestRequest("customers/" + customerToken, Method.POST);
            request.AddParameter("source", cardToken);

            var response = _stripeRestClient.Execute<StripeCustomer>(request);
            CheckStripeResponse("Customer update to add source failed", response);

            var defaultSourceId = response.Data.default_source;
            var sources = response.Data.sources.data;
            var defaultSource = MapDefaultSource(sources, defaultSourceId);
            
            return defaultSource;

        }

        public StripeCustomer AddSourceToCustomer(string customerToken, string cardToken)
        {
            //Passing source will create a new source object, make it the new customer default source, and delete the old customer default if one exist
            var request = new RestRequest("customers/" + customerToken + "/sources", Method.POST);
            request.AddParameter("source", cardToken);

            var response = _stripeRestClient.Execute<StripeCustomer>(request);
            CheckStripeResponse("Customer update to add source failed", response);
            var sourceData = response.Data;

            return sourceData;
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
                defaultSource.id = defaultSourceId;
            }

            return defaultSource;
        }

        public StripeCharge ChargeCustomer(string customerToken, int amount, int donorId)
        {
            var request = new RestRequest("charges", Method.POST);
            request.AddParameter("amount", amount * Constants.StripeDecimalConversionValue);
            request.AddParameter("currency", "usd");
            request.AddParameter("customer", customerToken);
            request.AddParameter("description", "Donor ID #" + donorId);
            request.AddParameter("expand[]", "balance_transaction");

            var response = _stripeRestClient.Execute<StripeCharge>(request);
            CheckStripeResponse("Invalid charge request", response);

            return response.Data;
        }

        public StripeCharge ChargeCustomer(string customerToken, string customerSourceId, int amount, int donorId)
        {
            var request = new RestRequest("charges", Method.POST);
            request.AddParameter("amount", amount * Constants.StripeDecimalConversionValue);
            request.AddParameter("currency", "usd");
            request.AddParameter("customer", customerToken);
            request.AddParameter("source", customerSourceId);
            request.AddParameter("description", "Donor ID #" + donorId);
            request.AddParameter("expand[]", "balance_transaction");

            var response = _stripeRestClient.Execute<StripeCharge>(request);
            CheckStripeResponse("Invalid charge request", response);

            return response.Data;
        }

        public List<StripeCharge> GetChargesForTransfer(string transferId)
        {
            var url = string.Format("transfers/{0}/transactions", transferId);
            var request = new RestRequest(url, Method.GET);
            request.AddParameter("count", _maxQueryResultsPerPage);

            var charges = new List<StripeCharge>();
            StripeCharges nextPage;
            do
            {
                var response = _stripeRestClient.Execute<StripeCharges>(request);
                CheckStripeResponse("Could not query transactions", response);

                nextPage = response.Data;
                charges.AddRange(nextPage.Data.Select(charge => charge));

                request = new RestRequest(url, Method.GET);
                request.AddParameter("count", _maxQueryResultsPerPage);
                request.AddParameter("starting_after", charges.Last().Id);
            } while (nextPage.HasMore);

            return (charges);
        }

        public StripeRefund GetChargeRefund(string chargeId)
        {
            var url = string.Format("charges/{0}/refunds", chargeId);
            var request = new RestRequest(url, Method.GET);

            var response = _stripeRestClient.Execute<StripeRefund>(request);
            CheckStripeResponse("Could not query charge refund", response);
            var refund = response.Data;
            
            return (refund);
        }

        public StripeCharge GetCharge(string chargeId)
        {
            var url = string.Format("charges/{0}", chargeId);
            var request = new RestRequest(url, Method.GET);
            request.AddParameter("expand[]", "balance_transaction");

            var response = _stripeRestClient.Execute(request);
            CheckStripeResponse("Could not query charge", response);

            // TODO Execute<StripeCharge>() above always gets an error deserializing the response, so using Execute() instead, and manually deserializing here
            var charge = JsonConvert.DeserializeObject<StripeCharge>(response.Content);

            return (charge);
        }

        public StripeRefundData GetRefund(string refundId)
        {
            var url = string.Format("refunds/{0}", refundId);
            var request = new RestRequest(url, Method.GET);
            request.AddParameter("expand[]", "charge");

            var response = _stripeRestClient.Execute(request);
            CheckStripeResponse("Could not query refund", response);

            // TODO Execute<StripeRefundData>() above always gets an error deserializing the response, so using Execute() instead, and manually deserializing here
            var refund = JsonConvert.DeserializeObject<StripeRefundData>(response.Content);

            return refund;
        }

        public StripePlan CreatePlan(RecurringGiftDto recurringGiftDto, ContactDonor contactDonor)
        {
            var request = new RestRequest("plans", Method.POST);
            request.AddParameter("amount", recurringGiftDto.PlanAmount * Constants.StripeDecimalConversionValue);
            request.AddParameter("interval", recurringGiftDto.PlanInterval);
            request.AddParameter("name", "Donor ID #" + contactDonor.DonorId + " " + recurringGiftDto.PlanInterval + "ly");
            request.AddParameter("currency", "usd");
            request.AddParameter("trial_period_days", recurringGiftDto.StartDate.Date.Subtract(DateTime.Today).Days);
            request.AddParameter("id", contactDonor.DonorId + " " + DateTime.Now);

            var response = _stripeRestClient.Execute<StripePlan>(request);
            CheckStripeResponse("Invalid plan creation request", response);

            return response.Data;
        }

        public StripeSubscription CreateSubscription(string planName, string customer)
        {
            var request = new RestRequest("customers/" + customer +"/subscriptions", Method.POST);
            request.AddParameter("plan", planName);

            var response = _stripeRestClient.Execute<StripeSubscription>(request);
            CheckStripeResponse("Invalid subscription creation request", response);

            return response.Data;
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

