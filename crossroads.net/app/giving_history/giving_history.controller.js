(function() {
  'use strict';
  module.exports = GivingHistoryController;

  GivingHistoryController.$inject = ['$log', 'GivingHistoryService', 'Profile'];

  function GivingHistoryController($log, GivingHistoryService, Profile) {
    var vm = this;

    vm.overall_view_ready = false;
    vm.beginning_donation_date = undefined;
    vm.currentDate = new Date();
    vm.donation_statement_total_amount = undefined;
    vm.donation_total_amount = undefined;
    vm.donation_years = [];
    vm.donations = [];
    vm.donation_history = false;
    vm.donation_view_ready = false;
    vm.ending_donation_date = undefined;
    vm.profile = {};
    vm.selected_giving_year = undefined;
    vm.soft_credit_donations = [];
    vm.soft_credit_donation_statement_total_amount = undefined;
    vm.soft_credit_donation_total_amount = undefined;
    vm.soft_credit_donation_history = false;
    vm.soft_credit_donation_view_ready = false;

    vm.getDonations = getDonations;
    vm.getSoftCreditDonations = getSoftCreditDonations;

    activate();

    function activate() {
      Profile.Personal.get(function(data) {
        vm.profile = data;
        GivingHistoryService.donationYears.get(function(data) {
          var most_recent_giving_year = data.most_recent_giving_year;

          // Create a map out of the array of donation years, so we can add an 'All' option easily,
          // and to facilitate ng-options on the frontend select
          vm.donation_years = _.transform(data.years, function(result, year) {
            result.push({key: year, value: year});
          });

          vm.donation_years.push({key: '', value: 'All'});

          // Set the default selected year based on the most recent giving year
          vm.selected_giving_year = _.find(vm.donation_years, {key: most_recent_giving_year});

          // Now get the donations for the selected year
          vm.overall_view_ready = true;
          vm.getDonations();
          vm.getSoftCreditDonations();
        },

        function(/*error*/) {
          vm.overall_view_ready = true;
          vm.donation_history = false;
          vm.soft_credit_donation_history = false;
        });
      },

      function(/*error*/) {
        vm.overall_view_ready = true;
        vm.donation_history = false;
        vm.soft_credit_donation_history = false;
      });
    }

    function getDonations() {
      vm.donation_view_ready = false;
      GivingHistoryService.donations.get({donationYear: vm.selected_giving_year.key}, function(data) {
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

    function getSoftCreditDonations() {
      vm.soft_credit_donation_view_ready = false;
      GivingHistoryService.donations.get({donationYear: vm.selected_giving_year.key, softCredit: true}, function(data) {
            vm.soft_credit_donations = data.donations;
            vm.soft_credit_donation_total_amount = data.donation_total_amount;
            vm.soft_credit_donation_statement_total_amount = data.donation_statement_total_amount;
            vm.soft_credit_donation_view_ready = true;
            vm.soft_credit_donation_history = true;
            vm.beginning_donation_date = data.beginning_donation_date;
            vm.ending_donation_date = data.ending_donation_date;
          },

          function(/*error*/) {
            vm.soft_credit_donation_history = false;
            vm.soft_credit_donation_view_ready = true;
          });
    }
  }
})();
