using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crds_angular.Services.Interfaces
{
    public interface IPaymentService
    {
        string createCustomer(string token);
        string chargeCustomer(string customer_token, int amount, int donor_id);
        string updateCustomerDescription(string customer_token, int donor_id);
        DefaultSource getDefaultSource(string customer_token);
    }

    public class DefaultSource
    {
        public string last4 { get; set; }
        public string brand { get; set; }
    }
}
