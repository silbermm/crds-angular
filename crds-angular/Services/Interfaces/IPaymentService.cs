﻿using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads.Stewardship;
using MinistryPlatform.Models;

namespace crds_angular.Services.Interfaces
{
    public interface IPaymentService
    {
        StripeCustomer CreateCustomer(string customerToken, string donorDescription = null);
        StripeCustomer GetCustomer(string customerId);
        StripeCustomer DeleteCustomer(string customerId);
        string CreateToken(string accountNumber, string routingNumber);
        StripeCharge ChargeCustomer(string customerToken, decimal amount, int donorId);
        StripeCharge ChargeCustomer(string customerToken, string customerSourceId, decimal amount, int donorId);
        string UpdateCustomerDescription(string customerToken, int donorId);
        SourceData UpdateCustomerSource(string customerToken, string cardToken);
        SourceData GetDefaultSource(string customerToken);
        SourceData GetSource(string customerId, string sourceId);

        List<StripeCharge> GetChargesForTransfer(string transferId);
        StripeRefund GetChargeRefund(string chargeId);
        StripeRefundData GetRefund(string refundId);
        StripeCharge GetCharge(string chargeId);
        StripePlan CreatePlan(RecurringGiftDto recurringGiftDto, ContactDonor contactDonor);
        StripeSubscription CreateSubscription(string planName, string customer, DateTime trialEndDate);
        StripeSubscription UpdateSubscriptionPlan(string customerId, string subscriptionId, string planId, DateTime? trialEndDate = null);
        StripeSubscription GetSubscription(string customerId, string subscriptionId);
        StripeCustomer AddSourceToCustomer(string customerToken, string cardToken);
        StripeSubscription CancelSubscription(string customerId, string subscriptionId);
        StripePlan CancelPlan(string planId);
    }
}
