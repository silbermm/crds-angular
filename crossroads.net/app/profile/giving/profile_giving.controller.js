(function() {
  'use strict';

  module.exports = ProfileGivingController;

  ProfileGivingController.$inject = ['$log', '$filter', 'GivingHistoryService'];

  function ProfileGivingController($log, $filter, GivingHistoryService) {
    var vm = this;
    vm.donations = [];
    vm.donation_history = false;
    vm.donation_view_ready = false;

    activate();

    function activate() {
      vm.donation_view_ready = false;
      GivingHistoryService.donations.get({limit: 3}, function(data) {
            vm.donations = data.donations;
            vm.donation_view_ready = true;
            vm.donation_history = true;
          },

          function(/*error*/) {
            vm.donation_history = false;
            vm.donation_view_ready = true;
          });
    }
  }
})();
