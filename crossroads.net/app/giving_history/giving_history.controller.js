(function() {
  'use strict';
  module.exports = GivingHistoryController;

  GivingHistoryController.$inject = ['$log', 'GivingHistoryService', 'Profile'];

  function GivingHistoryController($log, GivingHistoryService, Profile) {
    var vm = this;

    vm.donation_years = [];
    vm.donations = [];
    vm.donation_total_amount = undefined;
    vm.history = false;
    vm.initialized = false;
    vm.most_recent_giving_year = undefined;
    vm.profile = {};
    vm.selected_giving_year = undefined;

    activate();

    function activate() {
      Profile.Personal.get(function(data) {
        vm.profile = data;
        GivingHistoryService.donationYears.get(function(data) {
          vm.most_recent_giving_year = data.most_recent_giving_year;

          // Create a map out of the array of donation years, so we can add an 'All' option easily,
          // and to facilitate ng-options on the frontend select
          vm.donation_years = _.transform(data.years, function(result, year) {
            result.push({'key': year, 'value': year});
          });
          vm.donation_years.push({'key': '', 'value': 'All'});

          // Set the default selected year based on the most recent giving year
          vm.selected_giving_year = _.find(vm.donation_years, {'key': vm.most_recent_giving_year});
          GivingHistoryService.donations.get({donationYear: vm.most_recent_giving_year}, function(data) {
            // Pull distributions up to the donation level for easy ng-repeat usage
            vm.donations = _.transform(data.donations, function(result, donation) {
              _.forEach(donation.distributions, function(distribution) {
                result.push({'donation': donation, 'distribution': distribution});
              })
            });
            vm.donation_total_amount = data.donation_total_amount;
            vm.initialized = true;
            vm.history = true;
          },
          function(error) {
            vm.history = false;
            vm.initialized = true;
          });
        },
        function(error) {
          vm.history = false;
          vm.initialized = true;
        });
      },
      function(error) {
        vm.history = false;
        vm.initialized = true;
      });
    }
  }
})();
