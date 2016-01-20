using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Services.Interfaces;
using CrossroadsStripeOnboarding.Models;
using MinistryPlatform.Models;
using RecurringGift = CrossroadsStripeOnboarding.Models.RecurringGift;

namespace CrossroadsStripeOnboarding.Services
{
    public class StripePlansAndSubscriptions
    {

        private readonly IPaymentService _paymentService;
        private readonly int _additionalTrialPeriod;

        public StripePlansAndSubscriptions(IPaymentService paymentService)
        {
            _paymentService = paymentService;
            _additionalTrialPeriod = Int32.Parse(ConfigurationManager.AppSettings.Get("additonalTrialPeriodMonths"));
        }

        public Messages generate()
        {
            using (var stripeDB = new StripeOnboardingContext())
            using (var mpDB = new MinistryPlatformContext())
            {
                CreatePlanAndSubscriptions(mpDB, stripeDB);
                return Messages.PlansAndSubscriptionsSuccess;
            }
        }

        private void CreatePlanAndSubscriptions(MinistryPlatformContext mpDB, StripeOnboardingContext stripeDB)
        {
            foreach (var gift in GetActiveNoneProcessedRecurringGifts(mpDB))
            {
                var account = GetStripeAccount(stripeDB, gift.Donor.C__ExternalPersonID, gift.DonorAccount.Account_Number);
                CreatePlanAndSubscription(gift, account, mpDB, stripeDB);
            }
        }

        private IEnumerable<RecurringGift> GetActiveNoneProcessedRecurringGifts(MinistryPlatformContext db)
        {
            return
                (from r in db.RecurringGifts
                    where r.End_Date == new DateTime(2000, 01, 01) && r.DonorAccount.Processor_Account_ID == null
                    select r).ToList();
        }

        private StripeAccount GetStripeAccount(StripeOnboardingContext db, int? personId, string last4)
        {
            return
                (from a in db.StripeAccounts
                    where a.StripeCustomer.ExternalPersonId == personId.ToString() && a.Last4 == last4 && a.StripeCustomer.Imported == false
                    select a).FirstOrDefault();
        }

        private void CreatePlanAndSubscription(RecurringGift gift, StripeAccount account, MinistryPlatformContext mpDB, StripeOnboardingContext stripeDB)
        {
            var plan = _paymentService.CreatePlan(MapToRecurringGiftDto(gift), MapToContactDonor(gift));
            var subscription = _paymentService.CreateSubscription(plan.Id, account.StripeCustomer.CustomerId, GetStartDate(gift));
            UpdateRecurringGiftAndDonorAccount(gift, account, subscription, mpDB, stripeDB);

        }

        private void UpdateRecurringGiftAndDonorAccount(RecurringGift gift, StripeAccount account, StripeSubscription subscription,
            MinistryPlatformContext mpDB, StripeOnboardingContext stripeDB)
        {
            gift.Subscription_ID = subscription.Id;
            gift.DonorAccount.Processor_Account_ID = account.NewCardId;
            gift.DonorAccount.Processor_ID = account.StripeCustomer.CustomerId;
            mpDB.SaveChanges();

            account.StripeCustomer.Imported = true;
            stripeDB.SaveChanges();
        }

        private RecurringGiftDto MapToRecurringGiftDto(RecurringGift gift)
        {
            return new RecurringGiftDto
            {
                PlanInterval = gift.Frequency_ID == 1 ? PlanInterval.Weekly : PlanInterval.Monthly,
                PlanAmount = gift.Amount,
            };
        }

        private ContactDonor MapToContactDonor(RecurringGift gift)
        {
            return new ContactDonor
            {
                DonorId = gift.Donor_ID,
            };
        }

        private DateTime GetStartDate(RecurringGift gift)
        {
            return (gift.Frequency_ID == 1 ? GetStartDateForWeek() : GetStartForMonth(gift)).AddMonths(_additionalTrialPeriod);
        }

        private DateTime GetStartDateForWeek()
        {
            var days = DateTime.Today.DayOfWeek == System.DayOfWeek.Tuesday ? 7 : ((int)System.DayOfWeek.Tuesday - (int)DateTime.Today.DayOfWeek + 7) % 7;
            return DateTime.Today.AddDays(days);
        }

        private DateTime GetStartForMonth(RecurringGift gift)
        {
            return gift.Day_Of_Month == 5 ? GetStartForMonth5th() : GetStartForMonth20th();
        }

        private DateTime GetStartForMonth5th()
        {
            var today = DateTime.Today;
            var months = today.Day < 5 ? 0 : 1;
            var fifth = new DateTime(today.Year, today.Month, 5);
            return fifth.AddMonths(months);
        }

        private DateTime GetStartForMonth20th()
        {
            var today = DateTime.Today;
            var months = today.Day < 20 ? 0 : 1;
            var twentieth = new DateTime(today.Year, today.Month, 20);
            return twentieth.AddMonths(months);
        }
    }
}
