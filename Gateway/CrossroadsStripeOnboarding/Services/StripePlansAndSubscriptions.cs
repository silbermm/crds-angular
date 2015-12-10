using System;
using System.Collections.Generic;
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

        public StripePlansAndSubscriptions(IPaymentService paymentService)
        {
            _paymentService = paymentService;
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
            foreach (var gift in GetActiveRecurringGifts(mpDB))
            {
                var account = GetStripeAccount(stripeDB, gift.Donor.C__ExternalPersonID, gift.DonorAccount.Account_Number);
                CreatePlanAndSubscription(gift, account, mpDB, stripeDB);
            }
        }

        private IEnumerable<RecurringGift> GetActiveRecurringGifts(MinistryPlatformContext db)
        {
            return
                (from r in db.RecurringGifts
                    where r.End_Date == null
                    select r).ToList();
        }

        private StripeAccount GetStripeAccount(StripeOnboardingContext db, int? personId, string last4)
        {
            return
                (from a in db.StripeAccounts
                    where a.StripeCustomer.ExternalPersonId == personId.ToString() && a.Last4 == last4 && a.StripeCustomer.Imported == false
                    select a).SingleOrDefault();
        }

        private void CreatePlanAndSubscription(RecurringGift gift, StripeAccount account, MinistryPlatformContext mpDB, StripeOnboardingContext stripeDB)
        {
            var plan = _paymentService.CreatePlan(MapToRecurringGiftDto(gift), MapToContactDonor(gift));
            var subscription = _paymentService.CreateSubscription(plan.Name, account.StripeCustomer.CustomerId, GetStartDate(gift));
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
            return gift.Frequency_ID == 1 ? GetStartDateForWeek() : GetStartForMonth(gift);
        }

        private DateTime GetStartDateForWeek()
        {
            var days = DateTime.Today.DayOfWeek == DayOfWeek.Tuesday ? 7 : ((int)DayOfWeek.Tuesday - (int)DateTime.Today.DayOfWeek + 7) % 7;
            return DateTime.Today.AddDays(days);
        }

        private DateTime GetStartForMonth(RecurringGift gift)
        {
            return gift.Day_Of_Month == 5 ? GetStartForMonth5th() : GetStartForMonth20th();
        }

        private DateTime GetStartForMonth5th()
        {
            var months = DateTime.Today.Day < 5 ? 0 : 1;
            return DateTime.Today.AddMonths(months);
        }

        private DateTime GetStartForMonth20th()
        {
            var months = DateTime.Today.Day < 20 ? 0 : 1;
            return DateTime.Today.AddMonths(months);
        }
    }
}
