using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Models.Crossroads;

namespace crds_angular.Services.Interfaces
{
    public interface IPaymentService
    {
        string createCustomer(string token);
        string chargeCustomer(string customer_token, int amount, int donor_id);
        string updateCustomerDescription(string customer_token, int donor_id);
        SourceData updateCustomerSource(string customerToken, string cardToken);
        SourceData getDefaultSource(string customer_token);
    }

    public class DefaultSourceDTO
    {
        public string last4 { get; set; }
        public string brand { get; set; }
        public string name { get; set; }
        public string address_zip { get; set; }
    }
    
}
