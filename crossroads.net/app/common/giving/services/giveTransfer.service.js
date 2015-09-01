(function() {
  'use strict';

  module.exports = GiveTransferService;

  function GiveTransferService() {
    var transferObject = {
      reset: function() {
        this.account = '';
        this.amount = undefined;
        this.amountSubmitted = false;
        this.brand = '';
        this.ccNumberClass = '';
        this.changeAccountInfo = false;
        this.declinedPayment = false;
        this.donor = {};
        this.email = '';
        this.last4 = '';
        this.processing = false;
        this.processingChange = false;
        this.program = undefined;
        this.routing = '';
        this.savedPayment = '';
        this.view = '';
      }
    };
    transferObject.reset();

    return transferObject;
  }
})();
