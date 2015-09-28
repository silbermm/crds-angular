using System.Collections.Generic;
using crds_angular.Models.Crossroads.Stewardship;

namespace crds_angular.Services.Interfaces
{
    public interface IPaymentService
    {
        StripeCustomer CreateCustomer(string customerToken);
        string CreateToken(string accountNumber, string routingNumber);
        StripeCharge ChargeCustomer(string customerToken, int amount, int donorId);
        StripeCharge ChargeCustomer(string customerToken, string customerSourceId, int amount, int donorId);
        string UpdateCustomerDescription(string customerToken, int donorId);
        SourceData UpdateCustomerSource(string customerToken, string cardToken);
        SourceData GetDefaultSource(string customerToken);
        List<StripeCharge> GetChargesForTransfer(string transferId);
        StripeRefund GetChargeRefund(string chargeId);
        StripeRefundData GetRefund(string refundId);
        StripeCharge GetCharge(string chargeId);
        StripePlan CreatePlan(decimal planAmount, string planInterval, string planName, string donorId);
        StripeSubscription CreateSubscription(string planName, string customer);
    }
}
