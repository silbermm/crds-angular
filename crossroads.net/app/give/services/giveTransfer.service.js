(function () {
  angular.module('crossroads.give').factory('GiveTransferService',GiveTransferService);


    function GiveTransferService() {
        var transferObject = {
          reset: function() {
            this.account = '';
            this.amount = '';
            this.donor = '';
            this.email = '';
            this.program = '';
            this.routing = '';
            this.view = '';
            this.ccNumberClass = '';
            this.declinedPayment = false;
          }
        };
        transferObject.reset();

        return transferObject;
    }
})()
