(function() {
  'use strict';

  module.exports = TripsSignupController;

  TripsSignupController.$inject = ['$log', '$stateParams', '$location', '$anchorScroll'];

  function TripsSignupController($log, $stateParams, $location, $anchorScroll) {

    var vm = this;

    vm.friendlyPageTitle = $stateParams.tripLocation;
    vm.pageTitle = $stateParams.tripLocation;
    vm.tripName = '';
    vm.whyPlaceholder = '';

    activate();

    ////////////////////////////////
    //// IMPLEMENTATION DETAILS ////
    ////////////////////////////////
    function activate() {
      switch (vm.pageTitle) {
        case 'nola':
          vm.friendlyPageTitle = 'New Orleans';
          vm.tripName = '2015 July New Orleans Men\'s Trip';
          break;
        case 'south-africa':
          vm.friendlyPageTitle = 'South Africa';
          vm.tripName = '2015 Oct SA Topsy Trip';
          break;
        case 'india':
          vm.friendlyPageTitle = 'India';
          vm.tripName = '2015 SEPT India JA Annual';
          vm.whyPlaceholder = 'Please be specific. ' +
            'In instances where we have a limited number of spots, we strongly consider responses to this question.';
          break;
        case 'nicaragua':
          vm.friendlyPageTitle = 'Nicaragua';
          vm.tripName = '2015 Winter Kickoff Trip';
          break;
      }

    }
  }

})();
