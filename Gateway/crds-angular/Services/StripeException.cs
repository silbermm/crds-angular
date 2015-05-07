using System;

namespace crds_angular.Services
{
    public class StripeException : Exception
    {

        public StripeException( ) :
            base()
        { }

        public StripeException( string auxMessage ) :
            base(auxMessage)
        { }
    }
}