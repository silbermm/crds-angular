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
        string chargeCustomer(string customer_token, int amount, int donor_id, string pymt_type);
        string updateCustomerDescription(string customer_token, int donor_id);
        DefaultSourceDTO updateCustomerSource(string customerToken, string cardToken);
        DefaultSourceDTO getDefaultSource(string customer_token);
    }
}
