(function() {
  'use strict';
  module.exports = GivingHistoryController;

  GivingHistoryController.$inject = ['$log', 'GivingHistoryService'];

  function GivingHistoryController($log, GivingHistoryService) {
    var vm = this;

    vm.donation_years = [];
    vm.most_recent_giving_year = undefined;
    vm.donations = [];
    vm.donation_total_amount = undefined;

    activate();

    function activate() {
      GivingHistoryService.donationYears.get(function(data) {
        vm.donation_years = data.years;
        vm.most_recent_giving_year = data.most_recent_giving_year;
        GivingHistoryService.donations.get({donationYear: vm.most_recent_giving_year}, function(data) {
          vm.donations = data.donations;
          vm.donation_total_amount = data.donation_total_amount;
        });
      });

    }

  }
})();
