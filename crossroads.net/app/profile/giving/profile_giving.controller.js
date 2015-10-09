(function() {
  'use strict';

  module.exports = ProfileGivingController;

  ProfileGivingController.$inject = ['$log', '$filter', 'GivingHistoryService', 'RecurringGivingService'];

  function ProfileGivingController($log, $filter, GivingHistoryService, RecurringGivingService) {
    var vm = this;
    vm.donations = [];
    vm.donation_history = false;
    vm.donation_view_ready = false;

    vm.recurring_gifts = [];
    vm.recurring_giving = false;
    vm.recurring_giving_view_ready = false;

    activate();

    function activate() {
      vm.donation_view_ready = false;
      vm.recurring_giving_view_ready = false;

      GivingHistoryService.donations.get({limit: 3}, function(data) {
        vm.donations = data.donations;
        vm.donation_view_ready = true;
        vm.donation_history = true;
      }, function(/*error*/) {
        vm.donation_history = false;
        vm.donation_view_ready = true;
      });

      RecurringGivingService.recurringGifts.query(function(data){
        vm.recurring_gifts = data;
        vm.recurring_giving_view_ready = true;
        vm.recurring_giving = true;
      }, function(/*error*/) {
        vm.recurring_giving = false;
        vm.recurring_giving_view_ready = true;
      });
    }
  }
})();
