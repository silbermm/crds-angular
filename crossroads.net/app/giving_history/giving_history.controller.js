(function() {
  'use strict';
  module.exports = GivingHistoryController;

  GivingHistoryController.$inject = ['$log', 'GivingHistoryService'];

  function GivingHistoryController($log, GivingHistoryService) {
    var vm = this;

    vm.donations = [];
    vm.total_donation_amount = 0;

    activate();

    function activate() {
      GivingHistoryService.donations.get(function(data) {
        vm.donations = data.donations;
        vm.donation_total_amount = data.donation_total_amount;
      });
    }

  }
})();
