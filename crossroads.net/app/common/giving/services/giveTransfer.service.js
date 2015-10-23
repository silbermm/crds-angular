(function() {
  'use strict';

  module.exports = GiveTransferService;

  GiveTransferService.$inject = ['Session', 'User', '$filter'];

  function GiveTransferService(Session, User, $filter) {
    var transferObject = {
      reset: reset,
      loadRecurringDonationInformation: loadRecurringDonationInformation,
    };

    transferObject.reset();

    function reset() {
      transferObject.account = '';
      transferObject.amount = undefined;
      transferObject.amountSubmitted = false;
      transferObject.anonymous = false;
      transferObject.bankinfoSubmitted = false;
      transferObject.brand = '';
      transferObject.campaign = { campaignId: null, campaignName: null, pledgeDonorId: null };
      transferObject.ccNumberClass = '';
      transferObject.changeAccountInfo = false;
      transferObject.declinedPayment = false;
      transferObject.donor = {};
      transferObject.donorError = false;
      transferObject.email = undefined;
      transferObject.givingType = undefined;
      transferObject.initialized = false;
      transferObject.last4 = '';
      transferObject.message = null;
      transferObject.processing = false;
      transferObject.processingChange = false;
      transferObject.program = undefined;
      transferObject.routing = '';
      transferObject.savedPayment = '';
      transferObject.view = 'bank';
      transferObject.recurringStartDate = undefined;
      transferObject.recurringGiftId = undefined;

      if (!Session.isActive()) {
        User.email = '';
      }
    }

    function loadRecurringDonationInformation(donation, programsInput) {
      transferObject.reset();

      transferObject.recurringGiftId = donation.recurring_gift_id;
      transferObject.amount = donation.amount;
      transferObject.amountSubmitted = false;
      transferObject.bankinfoSubmitted = false;
      transferObject.changeAccountInfo = true;
      transferObject.brand = '#' + donation.source.icon;
      transferObject.ccNumberClass = donation.source.icon;
      transferObject.givingType = donation.interval;
      transferObject.initialized = true;
      transferObject.last4 = donation.source.last4;
      transferObject.program = $filter('filter')(programsInput, {ProgramId: donation.program})[0];
      transferObject.recurringStartDate = donation.start_date;
      transferObject.view = donation.source.type === 'CreditCard' ? 'cc' : 'bank';
      setupRecurringInterval(donation);
      setupRecurringDonor(donation);
    }

    function setupRecurringDonor(donation) {
      transferObject.donor = {
        id: donation.donor_id,
        default_source: {
          credit_card: {
            last4: null,
            brand: null,
            address_zip: null,
            exp_date: null,
          },
          bank_account: {
            routing: null,
            last4: null,
          },
        },
      };

      if (donation.source.type === 'CreditCard') {
        transferObject.donor.default_source.credit_card.last4 = donation.source.last4;
        transferObject.donor.default_source.credit_card.brand = donation.source.brand;
        transferObject.donor.default_source.credit_card.address_zip = donation.source.address_zip;
        transferObject.donor.default_source.credit_card.exp_date = moment(donation.source.exp_date).format('MMYY');
      } else {
        transferObject.donor.default_source.bank_account.last4 = donation.source.last4;
        transferObject.donor.default_source.bank_account.routing = donation.source.routing;
      }
    }

    function setupRecurringInterval(donation) {
      if (donation.interval !== null) {
        transferObject.interval = _.capitalize(donation.interval.toLowerCase()) + 'ly';
      }
    }

    return transferObject;
  }
})();
