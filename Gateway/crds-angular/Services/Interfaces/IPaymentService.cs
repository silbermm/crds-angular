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
        string CreateCustomer(string customerToken);
        string ChargeCustomer(string customerToken, int amount, int donorId, string paymentType);
        string UpdateCustomerDescription(string customerToken, int donorId);
        SourceData UpdateCustomerSource(string customerToken, string cardToken);
        SourceData GetDefaultSource(string customerToken);
        List<string> GetChargesForTransfer(string transferId);
    }
}
