(function() {
  'use strict';

  module.exports = TripsSignupController;

  TripsSignupController.$inject = ['$log', 'Session',  'Campaign', 'WorkTeams'];

  function TripsSignupController($log, Session, Campaign, WorkTeams) {

    var vm = this;

    vm.ageLimitReached = ageLimitReached;
    vm.campaign = Campaign;
    vm.currentPage = 1;
    vm.numberOfPages = 0;
    vm.pageTitle = vm.campaign.formName;
    vm.tripName = vm.campaign.name;
    vm.whyPlaceholder = '';
    vm.workTeams = WorkTeams;

    activate();

    ////////////////////////////////
    //// IMPLEMENTATION DETAILS ////
    ////////////////////////////////
    function activate() {
      switch (vm.pageTitle) {
        case 'GO NOLA Application':
          vm.friendlyPageTitle = 'New Orleans';
          vm.numberOfPages = 5;
          break;
        case 'GO South Africa Application':
          vm.friendlyPageTitle = 'South Africa';
          vm.numberOfPages = 6;
          break;
        case 'GO India Application':
          vm.friendlyPageTitle = 'India';
          vm.numberOfPages = 6;
          vm.whyPlaceholder = 'Please be specific. ' +
            'In instances where we have a limited number of spots, we strongly consider responses to this question.';
          break;
        case 'GO Nicaragua Application':
          vm.friendlyPageTitle = 'Nicaragua';
          vm.numberOfPages = 6;
          break;
      }
    }

    function ageLimitReached() {
      if (Session.exists('age') && Session.exists('age') < Campaign.ageLimit) {
        //Under age limit, check for exceptions
        if(Session.exists('userId') && _.includes(Campaign.ageExceptions, Number(Session.exists('userId')))) {
          return false;
        } else {
          return true;
        }
      }
      return false;
    }

  }

})();
