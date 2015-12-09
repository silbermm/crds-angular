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

        private readonly IPaymentService _paymentServiced;

        public StripePlansAndSubscriptions(IPaymentService paymentService)
        {
            _paymentServiced = paymentService;
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
                CreatePlanAndSubscription(gift, account);
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
                    where a.StripeCustomer.ExternalPersonId == personId.ToString() && a.Last4 == last4
                    select a).SingleOrDefault();
        }

        private void CreatePlanAndSubscription(RecurringGift gift, StripeAccount account)
        {
            _paymentServiced.CreatePlan(MapToRecurringGiftDto(gift), MapToContactDonor(gift));
            //CreateSubscription(string planName, string customer, DateTime trialEndDate)
            MapToRecurringGiftDto(gift);
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
    }
}
