using System;
using System.Net;
using crds_angular.Models.Json;
using Newtonsoft.Json;

namespace crds_angular.Exceptions
{
    public class StripeException : Exception
    {
        public string code { get; set; }
        public string detailMessage { get; set; }
        public string type { get; set; }
        public string DeclineCode { get; set; }

        public StripeException(HttpStatusCode statusCode, string auxMessage, string type, string message, string code, string declineCode) :
            base(auxMessage)
        {
            this.type = type;
            this.detailMessage = message;
            this.code = code;
            this.DeclineCode = declineCode;
        }

        public RestHttpActionResult<StripeErrorResponse> GetStripeResult()
        {
            var stripeError = new StripeErrorResponse
            {
                Error = new StripeError
                {
                    DeclineCode = DeclineCode,
                    Code = code,
                    Message = detailMessage,
                    Type = type
                }
            };
            return (RestHttpActionResult<StripeErrorResponse>.WithStatus(stripeError, HttpStatusCode.PaymentRequired));
        }
    }

    public class StripeErrorResponse
    {
        [JsonProperty(PropertyName = "error")]
        public StripeError Error { get; set; }
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