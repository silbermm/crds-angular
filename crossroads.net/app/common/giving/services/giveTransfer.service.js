(function() {
  'use strict';

  module.exports = GiveTransferService;

  GiveTransferService.$inject = ['Session', 'User'];

  function GiveTransferService(Session, User) {
    var transferObject = {
      reset: function() {
        this.account = '';
        this.amount = undefined;
        this.amountSubmitted = false;
        this.anonymous = false;
        this.bankinfoSubmitted = false;
        this.brand = '';
        this.campaign = { campaignId: null, campaignName: null, pledgeDonorId: null };
        this.ccNumberClass = '';
        this.changeAccountInfo = false;
        this.declinedPayment = false;
        this.donor = {};
        this.donorError = false;
        this.email = undefined;
        this.givingType = undefined;
        this.initialized = false;
        this.last4 = '';
        this.message = null;
        this.processing = false;
        this.processingChange = false;
        this.program = undefined;
        this.routing = '';
        this.savedPayment = '';
        this.view = 'bank';
        this.recurringStartDate = undefined;
        this.recurringGiftId = undefined;
        this.impersonateDonorId = undefined;

        if (!Session.isActive()) {
          User.email = '';
        }

      }
    };
    transferObject.reset();

    return transferObject;
  }
})();
