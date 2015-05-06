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
        string chargeCustomer(string customer_token, int amount, string donor_id);
    }
}
