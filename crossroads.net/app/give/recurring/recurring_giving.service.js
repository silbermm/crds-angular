(function() {
  'use strict';

  module.exports = RecurringGiving;

  RecurringGiving.$inject = ['GiveTransferService', 'DonationService', 'GiveFlow', 'Session', '$state', '$filter'];

  function RecurringGiving(GiveTransferService, DonationService, GiveFlow, Session, $state, $filter) {
    var service = {
      name: 'RecurringGiving',
      initDefaultState: initDefaultState,
      resetGiveFlow: resetGiveFlow,
      goToAccount: goToAccount,
      stateName: stateName,
      goToChange: goToChange,
      goToLogin: goToLogin,
      submitBankInfo: submitBankInfo,
      processChange: processChange,
      getLoggedInUserDonorPaymentInfo: getLoggedInUserDonorPaymentInfo,
      resetGiveTransferServiceGiveType: resetGiveTransferServiceGiveType,
      loadDonationInformation: loadDonationInformation,
    };

    function initDefaultState() {
      GiveTransferService.reset();
      GiveTransferService.processing = false;

      resetGiveFlow();
      GiveTransferService.initialized = true;
      GiveTransferService.givingType = 'month';

      Session.removeRedirectRoute();
      $state.go(GiveFlow.amount);
    }

    function resetGiveFlow() {
      // Setup the give flow service
      GiveFlow.reset({
        amount: 'give.amount',
        account: 'give.recurring_account',
        login: 'give.recurring_login',
        register: 'give.register',
        confirm: 'give.recurring_account',
        change: 'give.recurring_change',
        thankYou: 'give.recurring_thank-you'
      });
    }

    function resetGiveTransferServiceGiveType() {
      GiveTransferService.givingType = 'month';
    }

    function stateName(state) {
      return GiveFlow[state];
    }

    function goToAccount(giveForm) {
      GiveFlow.goToAccount(giveForm);
    }

    function goToChange() {
      GiveFlow.goToChange();
    }

    function goToLogin() {
      GiveFlow.goToLogin();
    }

    function submitBankInfo(giveForm, programsInput) {
      DonationService.createRecurringGift();
    }

    function processChange() {
      DonationService.processChange();
    }

    function getLoggedInUserDonorPaymentInfo(event, toState) {
    }

    function loadDonationInformation(programsInput, donation = null, impersonateDonorId = null) {
      GiveTransferService.reset();

      GiveTransferService.amountSubmitted = false;
      GiveTransferService.bankinfoSubmitted = false;
      GiveTransferService.changeAccountInfo = true;
      GiveTransferService.initialized = true;
      setupDonor(donation, impersonateDonorId);

      if (donation !== null) {
        GiveTransferService.recurringGiftId = donation.recurring_gift_id;
        GiveTransferService.amount = donation.amount;
        GiveTransferService.brand = '#' + donation.source.icon;
        GiveTransferService.ccNumberClass = donation.source.icon;
        GiveTransferService.givingType = donation.interval;
        GiveTransferService.last4 = donation.source.last4;
        GiveTransferService.program = $filter('filter')(programsInput, {ProgramId: donation.program})[0];
        GiveTransferService.recurringStartDate = donation.start_date;
        GiveTransferService.view = donation.source.type === 'CreditCard' ? 'cc' : 'bank';
        setupInterval(donation);
      }
    }

    function setupDonor(donation, impersonateDonorId = null) {
      GiveTransferService.donor = {
        id: (impersonateDonorId == null ? donation.donor_id : impersonateDonorId),
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
        GiveTransferService.donor.default_source.credit_card.last4 = donation.source.last4;
        GiveTransferService.donor.default_source.credit_card.brand = donation.source.brand;
        GiveTransferService.donor.default_source.credit_card.address_zip = donation.source.address_zip;
        GiveTransferService.donor.default_source.credit_card.exp_date = moment(donation.source.exp_date).format('MMYY');
      } else {
        GiveTransferService.donor.default_source.bank_account.last4 = donation.source.last4;
        GiveTransferService.donor.default_source.bank_account.routing = donation.source.routing;
      }
    }

    function setupInterval(donation) {
      if (donation.interval !== null) {
        GiveTransferService.interval = _.capitalize(donation.interval.toLowerCase()) + 'ly';
      }
    }

    return service;
  }

})();

