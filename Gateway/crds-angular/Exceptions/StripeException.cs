using System;
using System.Net;
using crds_angular.Models.Json;
using Newtonsoft.Json;

namespace crds_angular.Exceptions
{
    public class StripeException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Code { get; set; }
        public string DetailMessage { get; set; }
        public string Type { get; set; }
        public string DeclineCode { get; set; }
        public string Param { get; set; }

        public StripeException(HttpStatusCode statusCode, string auxMessage, string type, string message, string code, string declineCode, string param) :
            base(auxMessage)
        {
            Type = type;
            DetailMessage = message;
            Code = code;
            DeclineCode = declineCode;
            StatusCode = statusCode;
            Param = param;
        }

        public RestHttpActionResult<StripeErrorResponse> GetStripeResult()
        {
            var stripeError = new StripeErrorResponse
            {
                Error = new StripeError
                {
                    DeclineCode = DeclineCode,
                    Code = Code,
                    Message = DetailMessage,
                    Type = Type,
                    Param = Param
                }
            };
            return (RestHttpActionResult<StripeErrorResponse>.WithStatus(StatusCode, stripeError));
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