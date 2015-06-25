using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using crds_angular.Services;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Exceptions
{
    public class StripeException : Exception
    {
        public string code { get; set; }
        public string detailMessage { get; set; }
        public string type { get; set; }
        public string DeclineCode { get; set; }

        public StripeException(string auxMessage, string type, string message, string code, string declineCode) :
            base(auxMessage)
        {
            this.type = type;
            this.detailMessage = message;
            this.code = code;
            this.DeclineCode = declineCode;
        }

        public PaymentRequiredResult GetPaymentRequiredResult()
        {
            return (new PaymentRequiredResult(this));
        }
    }

    public class PaymentRequiredResult : IHttpActionResult
    {
        private readonly StripeException _stripeException;

        public PaymentRequiredResult(StripeException stripeException)
        {
            _stripeException = stripeException;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.PaymentRequired);
            var stripeError = new StripeError
            {
                DeclineCode = _stripeException.DeclineCode,
                Code = _stripeException.code,
                Message = _stripeException.detailMessage,
                Type = _stripeException.type
            };
            response.Content = new StringContent(JsonConvert.SerializeObject(new { error = stripeError }));
            return (Task.FromResult(response));
        }
    }

    public class StripeError
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
}