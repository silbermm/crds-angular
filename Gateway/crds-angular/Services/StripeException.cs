using System;

namespace crds_angular.Services
{
    public class StripeException : Exception
    {
        public Error error { get; set; }

        public StripeException(string auxMessage, Error error) :
            base(auxMessage)
        {
            this.error = error;
        }
    }
}