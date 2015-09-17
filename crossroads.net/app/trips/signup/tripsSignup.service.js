(function() {
  'use strict';

  module.exports = TripsSignupService;

  TripsSignupService.$inject = ['$resource', '$location'];

  function TripsSignupService($resource, $location) {
    var signupService = {
      activate: activate,
      reset: reset,
      thankYouMessage: ''
    };

    function activate() {
      switch (signupService.campaign.nickname) {
        case 'GO NOLA Application':
          signupService.friendlyPageTitle = 'New Orleans';
          signupService.tripName = '';
          signupService.numberOfPages = 5;
          break;
        case 'South Africa':
          signupService.friendlyPageTitle = 'South Africa';
          signupService.tripName = '';
          signupService.numberOfPages = 6;
          break;
        case 'India':
          signupService.friendlyPageTitle = 'India';
          signupService.tripName = '';
          signupService.numberOfPages = 6;
          signupService.whyPlaceholder = 'Please be specific. ' +
            'In instances where we have a limited number of spots, we strongly consider responses to this question.';
          break;
        case 'GO Nicaragua Application':
          signupService.friendlyPageTitle = 'Nicaragua';
          signupService.tripName = '';
          signupService.numberOfPages = 6;
          break;
      }
    }

    function reset(campaign) {
      signupService.campaign = campaign;
      signupService.ageLimitReached = false;
      signupService.contactId = '';
      signupService.currentPage = 1;
      signupService.numberOfPages = 0;
      signupService.pageHasErrors = true;
      signupService.privateInvite = $location.search().invite;
    }

    return signupService;
  }
})();
