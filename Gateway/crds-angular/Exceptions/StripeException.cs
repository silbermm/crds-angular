using System;
using crds_angular.Services;

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


    }
}