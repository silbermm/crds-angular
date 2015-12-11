using System;
using System.Linq;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Models.Json;
using crds_angular.Services.Interfaces;
using CrossroadsStripeOnboarding.Models;
using log4net;

namespace CrossroadsStripeOnboarding.Services
{
    public class VerifyStripeSubscriptions
    {
        private const string Null = "NULL";

        private static readonly ILog Logger = LogManager.GetLogger(typeof (VerifyStripeSubscriptions));
        private static readonly ILog VerifyOutput = LogManager.GetLogger("VERIFY_OUTPUT");

        private readonly MinistryPlatformContext _mpContext;
        private readonly IPaymentService _paymentService;

        public VerifyStripeSubscriptions(MinistryPlatformContext mpContext, IPaymentService paymentService)
        {
            _mpContext = mpContext;
            _paymentService = paymentService;
        }

        public void Verify()
        {
            Logger.Info("Starting verification process");
            var recurringGifts = _mpContext.RecurringGifts.ToList();
            var giftsToProcess = recurringGifts.Count();
            Logger.Info(string.Format("Verifying {0} recurring gifts", giftsToProcess));

            VerifyOutput.Info("mpRecurringGiftId,stripeSubscriptionId,mpAmount,stripeAmount,mpFrequency,stripeFrequency,mpRepeat,stripeRepeat,mpDonorAccountType,stripeAccountType,mpAccountLast4,stripeAccountLast4,success,errorMessage");

            var giftsProcessed = 0;

            foreach (var gift in recurringGifts)
            {
                Logger.Info(string.Format("Processing gift #{0} ({1}%)", ++giftsProcessed, (giftsProcessed / giftsToProcess) * 100M));

                var success = true;
                var errorMessage = string.Empty; 

                StripeSubscription sub = null;
                var mpRecurringGiftId = gift.Recurring_Gift_ID;
                var mpAmount = gift.Amount;
                var mpFrequency = gift.Frequency.ToString();
                var mpRepeat = gift.Frequency == Frequency.Monthly ? gift.Day_Of_Month + "" : gift.DayOfWeek.ToString();
                var mpDonorAccountType = gift.DonorAccount != null ? gift.DonorAccount.AccountType.ToString() : Null;
                var mpAccountLast4 = gift.DonorAccount != null ? gift.DonorAccount.Account_Number : Null;

                if (gift.DonorAccount == null || string.IsNullOrWhiteSpace(gift.DonorAccount.Processor_ID) || string.IsNullOrWhiteSpace(gift.Subscription_ID))
                {
                    success = false;
                    errorMessage = "Recurring gift is missing Stripe processor information";
                }
                else
                {
                    try
                    {
                        sub = _paymentService.GetSubscription(gift.DonorAccount.Processor_ID, gift.Subscription_ID);
                    }
                    catch (Exception e)
                    {
                        success = false;
                        errorMessage = string.Format("Could not lookup subscription for customer {0}, subscription {1}: {2}",
                                                     gift.DonorAccount.Processor_ID,
                                                     gift.Subscription_ID,
                                                     e.Message);
                        Logger.Error(errorMessage, e);
                    }
                }

                var stripeSubscriptionId = sub == null ? Null : sub.Id;
                var stripeAmount = sub == null || sub.Plan == null ? 0M : sub.Plan.Amount;
                var freq = sub == null || sub.Plan == null
                    ? null
                    : sub.Plan.Interval.ToLower().Equals("month") ? Frequency.Monthly : sub.Plan.Interval.ToLower().Equals("week") ? Frequency.Weekly : (Frequency?) null;
                var stripeFrequency = freq == null ? Null : freq.ToString();
                var startDate = sub == null || string.IsNullOrWhiteSpace(sub.Start) ? (DateTime?)null : StripeEpochTime.ConvertEpochToDateTime(long.Parse(sub.Start));
                var stripeRepeat = freq != null && freq == Frequency.Monthly && startDate != null
                    ? startDate.Value.Day+""
                    : freq != null && freq == Frequency.Weekly && startDate != null ? startDate.Value.DayOfWeek.ToString() : Null;
                var stripeAccountType = Null;
                var stripeAccountLast4 = Null;
                if (sub != null && !string.IsNullOrWhiteSpace(sub.Customer))
                {
                    try
                    {
                        var source = _paymentService.GetDefaultSource(sub.Customer);
                        if("card".Equals(source.@object))
                        {
                            stripeAccountType = AccountType.CreditCard.ToString();
                            stripeAccountLast4 = source.last4;
                        }
                        else if ("bank".Equals(source.@object))
                        {
                            stripeAccountType = AccountType.Bank.ToString();
                            stripeAccountLast4 = source.bank_last4;
                        }
                    }
                    catch (Exception e)
                    {
                        success = false;
                        errorMessage = string.Format("Could not lookup default source for customer {0}: {1}", sub.Customer, e.Message);
                        Logger.Error(errorMessage, e);
                    }
                }

                if (success)
                {
                    success = mpAmount == stripeAmount && mpFrequency.Equals(stripeFrequency) && mpRepeat.Equals(stripeRepeat) &&
                              mpDonorAccountType.Equals(stripeAccountType) && mpAccountLast4.Equals(stripeAccountLast4);
                    if (!success)
                    {
                        errorMessage = "Field mismatch";
                    }
                }

                LogRecurringGiftComparison(mpRecurringGiftId,
                                      stripeSubscriptionId,
                                      mpAmount,
                                      stripeAmount,
                                      mpFrequency,
                                      stripeFrequency,
                                      mpRepeat,
                                      stripeRepeat,
                                      mpDonorAccountType,
                                      stripeAccountType,
                                      mpAccountLast4,
                                      stripeAccountLast4,
                                      success,
                                      errorMessage);
            }
        }

        private static void LogRecurringGiftComparison(int mpRecurringGiftId,
                                                  string stripeSubscriptionId,
                                                  decimal mpAmount,
                                                  decimal stripeAmount,
                                                  string mpFrequency,
                                                  string stripeFrequency,
                                                  string mpRepeat,
                                                  string stripeRepeat,
                                                  string mpDonorAccountType,
                                                  string stripeAccountType,
                                                  string mpAccountLast4,
                                                  string stripeAccountLast4,
                                                  bool success,
                                                  string errorMessage)
        {
            VerifyOutput.Info(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}",
                                            mpRecurringGiftId,
                                            stripeSubscriptionId,
                                            mpAmount,
                                            stripeAmount,
                                            mpFrequency,
                                            stripeFrequency,
                                            mpRepeat,
                                            stripeRepeat,
                                            mpDonorAccountType,
                                            stripeAccountType,
                                            mpAccountLast4,
                                            stripeAccountLast4,
                                            success,
                                            errorMessage));
        }
    }
}
