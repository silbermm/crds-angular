using System;
using System.Net;
using crds_angular.Models.Json;
using Crossroads.Utilities.Models;
using Newtonsoft.Json;

namespace crds_angular.Exceptions
{
    public class PaymentProcessorException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Code { get; set; }
        public string DetailMessage { get; set; }
        public string Type { get; set; }
        public string DeclineCode { get; set; }
        public string Param { get; set; }
        public ContentBlock GlobalMessage { get; set; }

        public PaymentProcessorException(HttpStatusCode statusCode, string auxMessage, string type, string message, string code, string declineCode, string param, ContentBlock globalMessage = null) :
            base(auxMessage)
        {
            Type = type;
            DetailMessage = message;
            Code = code;
            DeclineCode = declineCode;
            StatusCode = statusCode;
            Param = param;
            GlobalMessage = globalMessage;
        }

        public RestHttpActionResult<PaymentProcessorErrorResponse> GetStripeResult()
        {
            var stripeError = new PaymentProcessorErrorResponse
            {
                Error = new PaymentProcessorError
                {
                    DeclineCode = DeclineCode,
                    Code = Code,
                    Message = DetailMessage,
                    Type = Type,
                    Param = Param,
                    GlobalMessage = GlobalMessage
                }
            };
            return (RestHttpActionResult<PaymentProcessorErrorResponse>.WithStatus(StatusCode, stripeError));
        }
    }

    public class PaymentProcessorErrorResponse
    {
        [JsonProperty(PropertyName = "error")]
        public PaymentProcessorError Error { get; set; }
    }

    public class PaymentProcessorError
    {
        // Properties are here for debugging purposes, but will not be serialized into JSON;
        // Only the GlobalMessage will be serialized.
        [JsonIgnore, JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonIgnore, JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        [JsonIgnore, JsonProperty(PropertyName = "param")]
        public string Param { get; set; }
        [JsonIgnore, JsonProperty(PropertyName = "code")]
        public string Code { get; set; }
        [JsonIgnore, JsonProperty(PropertyName = "decline_code")]
        public string DeclineCode { get; set; }
        [JsonProperty(PropertyName = "globalMessage", NullValueHandling = NullValueHandling.Ignore)]
        public ContentBlock GlobalMessage { get; set; }
    }
}