(function() {
  'use strict';
  module.exports = GivingHistoryController;

  GivingHistoryController.$inject = ['$log', 'GivingHistoryService', 'Profile'];

  function GivingHistoryController($log, GivingHistoryService, Profile) {
    var vm = this;

    vm.currentDate = new Date();
    vm.donation_years = [];
    vm.donations = [];
    vm.donation_total_amount = undefined;
    vm.getDonations = getDonations;
    vm.history = false;
    vm.most_recent_giving_year = undefined;
    vm.profile = {};
    vm.selected_giving_year = undefined;
    vm.viewReady = false;

    activate();

    function activate() {
      Profile.Personal.get(function(data) {
        vm.profile = data;
        GivingHistoryService.donationYears.get(function(data) {
          vm.most_recent_giving_year = data.most_recent_giving_year;

          // Create a map out of the array of donation years, so we can add an 'All' option easily,
          // and to facilitate ng-options on the frontend select
          vm.donation_years = _.transform(data.years, function(result, year) {
            result.push({key: year, value: year});
          });

          vm.donation_years.push({key: '', value: 'All'});

          // Set the default selected year based on the most recent giving year
          vm.selected_giving_year = _.find(vm.donation_years, {key: vm.most_recent_giving_year});

          // Now get the donations for the selected year
          vm.getDonations();
        },

        function(/*error*/) {
          vm.history = false;
          vm.viewReady = true;
        });
      },

      function(/*error*/) {
        vm.history = false;
        vm.viewReady = true;
      });
    }

    function getCardIcon(brand) {
      switch (brand) {
        case 'Visa':
          return ('cc_visa');
        case 'MasterCard':
          return ('cc_mastercard');
        case 'Discover':
          return ('cc_discover');
        case 'AmericanExpress':
          return ('cc_american_express');
        default:
          return ('');
      }
    }

    function getDonations() {
      vm.viewReady = false;
      GivingHistoryService.donations.get({donationYear: vm.selected_giving_year.key}, function(data) {
            vm.donations = postProcessDonations(data.donations);
            vm.donation_total_amount = data.donation_total_amount;
            vm.viewReady = true;
            vm.history = true;
          },

          function(/*error*/) {
            vm.viewReady = true;
            vm.history = false;
          });
    }

    function postProcessDonations(donations) {
      _.forEach(donations, function(donation) {
        setDonationDisplayDetails(donation.source);
      });

      return (donations);
    }

    function setDonationDisplayDetails(source) {
      switch (source.type) {
        case 'Cash':
          source.icon = 'money';
          source.viewBox = '0 0 34 32';
          break;
        case 'Bank':
        case 'Check':
          source.icon = 'library';
          source.viewBox = '0 0 32 32';
          source.name = 'ending in ' + source.last4;
          break;
        case 'CreditCard':
          source.icon = getCardIcon(source.brand);
          source.viewBox = '0 0 160 100';
          source.name = 'ending in ' + source.last4;
          break;
      }
    }
  }
})();
