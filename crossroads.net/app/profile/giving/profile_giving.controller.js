(function() {
  'use strict';

  module.exports = ProfileGivingController;

  ProfileGivingController.$inject = ['$log', 'GivingHistoryService'];

  function ProfileGivingController($log, GivingHistoryService) {
    var vm = this;
    vm.donation_view_ready = false;
    vm.donations = [];
    vm.donation_total_amount = undefined;
    vm.donation_history = false;
    vm.donation_view_ready = false;

    activate();

    function activate() {
      vm.donation_view_ready = false;
      GivingHistoryService.donations.get({}, function(data) {
            vm.donations = data.donations;
            vm.donation_total_amount = data.donation_total_amount;
            vm.donation_statement_total_amount = data.donation_statement_total_amount;
            vm.donation_view_ready = true;
            vm.donation_history = true;
            vm.beginning_donation_date = data.beginning_donation_date;
            vm.ending_donation_date = data.ending_donation_date;
          },

          function(/*error*/) {
            vm.donation_history = false;
            vm.donation_view_ready = true;
          });
    }
  }
})();
