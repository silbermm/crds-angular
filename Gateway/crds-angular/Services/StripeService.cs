using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using crds_angular.Models;
using crds_angular.Services.Interfaces;
using RestSharp;

namespace crds_angular.Services
{
    public class StripeService : IStripeService
    {
        public string createCustomer(string token)
        {
            // TODO: read from configuration
            var client = new RestClient("https://api.stripe.com/v1");
            // TODO: read from environment variable e.g. MP login info
            client.Authenticator = new HttpBasicAuthenticator("sk_test_13Lo24dJijtpqzZwOZDbOL7C", null);

            var request = new RestRequest("customers", Method.POST);
            request.AddParameter("description", "testing customers"); // adds to POST or URL querystring based on Method
            request.AddParameter("source", token);
 
            RestResponse<StripeCustomer> response = (RestResponse<StripeCustomer>) client.Execute<StripeCustomer>(request);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // TODO: deserialize content into StripeError and return message in StripeException
                throw new StripeException();
            } 

            return response.Data.id;
          
        }
    }
}