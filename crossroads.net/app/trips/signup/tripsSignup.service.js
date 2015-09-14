(function() {
  'use strict';

  module.exports = TripsSignupService;

  TripsSignupService.$inject = ['$resource', '$location', '$log', 'Session'];

  function TripsSignupService($resource, $location, $log, Session) {
    var signupService = {
      activate: activate,
      reset: reset,
      TripApplication: $resource(__API_ENDPOINT__ + 'api/trip-application')
    };

    function activate() {
      $log.debug('signup service activate');

      signupService.contactId = Session.exists('userId');
      signupService.pledgeCampaignId = 178;

      if (signupService.page2 === undefined) {
        $log.debug('init fields');

        var page2 = {
          guardianFirstName: {formFieldId: 1221, value: null},
          guardianLastName: {formFieldId: 1222, value: null},
          tshirtSize: null,
          scrubSize: null,
          referral: {formFieldId: 1229, value: null},
          conditions: {formFieldId: 1227, value: null},
          vegetarian: {formFieldId: 1225, value: null},
          allergies: {formFieldId: 1226, value: null},
          spiritualLifeSearching: {formFieldId: 1231, value: null},
          spiritualLifeReceived: {formFieldId: 1232, value: null},
          spiritualLifeObedience: {formFieldId: 1233, value: null},
          spiritualLifeReplicating: {formFieldId: 1234, value: null},
          why: {formFieldId: 1230, value: null}
        };
        signupService.page2 = page2;
      }

      // switch (signupService.campaign.formName) {
      //   case 'GO NOLA Application':
      //     signupService.friendlyPageTitle = 'New Orleans';
      //     signupService.tripName = '';
      //     signupService.numberOfPages = 5;
      //     break;
      //   case 'GO South Africa Application':
      //     signupService.friendlyPageTitle = 'South Africa';
      //     signupService.tripName = '';
      //     signupService.numberOfPages = 6;
      //     break;
      //   case 'GO India Application':
      //     signupService.friendlyPageTitle = 'India';
      //     signupService.tripName = '';
      //     signupService.numberOfPages = 6;
      //     signupService.whyPlaceholder = 'Please be specific. ' +
      //       'In instances where we have a limited number of spots, we strongly consider responses to this question.';
      //     break;
      //   case 'GO Nicaragua Application':
      //     signupService.friendlyPageTitle = 'Nicaragua';
      //     signupService.tripName = '';
      //     signupService.numberOfPages = 6;
      //     break;
      // }
    }

    function reset(campaign) {
      signupService.campaign = campaign;
      signupService.ageLimitReached = false;
      signupService.contactId = '';
      signupService.currentPage = 1;
      signupService.numberOfPages = 0;
      signupService.pageHasErrors = true;
      signupService.privateInvite = $location.search().invite;

      //signupService.page2 = resetPageTwo();

    }

    // function resetPageTwo() {
    //   var p2 = {};
    //   p2.guardianFirstName = '';
    //   p2.guardianLastName = '';
    //   p2.tshirtSize = '';
    //   p2.scrubSize = '';
    //   p2.referral = '';
    //   p2.conditions = '';
    //   p2.allergies = '';
    //   p2.spiritualLifeSearching = '';
    //   p2.spiritualLifeReceived = '';
    //   p2.spiritualLifeObedience = '';
    //   p2.spiritualLifeReplicating = '';
    //   p2.why = '';
    //   return p2;
    // }

    function saveApplication() {
      $log.debug(signupService.page2);
    }

    return signupService;
  }
})();
