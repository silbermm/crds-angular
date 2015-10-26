(function() {
  'use strict';

  module.exports = TripsSignupService;

  TripsSignupService.$inject = ['$resource', '$location', '$log', 'Session'];

  function TripsSignupService($resource, $location, $log, Session) {
    var signupService = {
      activate: activate,
      pages: [],
      reset: reset,
      TripApplication: $resource(__API_ENDPOINT__ + 'api/trip-application'),
      thankYouMessage: '',
    };

    function activate() {
      $log.debug('signup service activate');

      if (signupService.page2 === undefined) {
        signupService.page2 = page2();
      }

      if (signupService.page3 === undefined) {
        signupService.page3 = page3();
      }

      if (signupService.page4 === undefined) {
        signupService.page4 = page4();
      }

      if (signupService.page5 === undefined) {
        signupService.page5 = page5();
      }

      if (signupService.page6 === undefined) {
        signupService.page6 = page6();
      }

      //relying on Pledge Campaign Nickname field feels very fragile, is there another way?
      signupService.friendlyPageTitle = signupService.campaign.nickname;
      switch (signupService.campaign.nickname) {
        case 'NOLA':
          signupService.numberOfPages = 5;
          break;
        case 'South Africa':
          signupService.numberOfPages = 6;
          break;
        case 'India':
          signupService.numberOfPages = 6;
          signupService.whyPlaceholder = 'Please be specific. ' +
            'In instances where we have a limited number of spots, we strongly consider responses to this question.';
          break;
        case 'Nicaragua':
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

      signupService.page2 = page2();
      signupService.page3 = page3();
      signupService.page4 = page4();
      signupService.page5 = page5();
      signupService.page6 = page6();
    }

    function page2() {
      return {
        guardianFirstName: null,
        guardianLastName: null,
        referral: null,
        conditions: null,
        why: null,
      };
    }

    function page3() {
      return {
        emergencyContactFirstName: null,
        emergencyContactLastName: null,
        emergencyContactEmail: null,
        emergencyContactPrimaryPhone: null,
        emergencyContactSecondaryPhone: null
      };
    }

    function page4() {
      return {
        lottery: null,
        groupCommonName: null,
        roommateFirstChoice: null,
        roommateSecondChoice: null,
        supportPersonEmail: null,
        interestedInGroupLeader: null,
        whyGroupLeader: null,
      };
    }

    function page5() {
      return {
        sponsorChildInNicaragua: null,
        sponsorChildFirstName: null,
        sponsorChildLastName: null,
        sponsorChildNumber: null,
        sponsorChildTown: null,
        nolaFirstChoiceWorkTeam: null,
        nolaFirstChoiceExperience: null,
        nolaSecondChoiceWorkTeam: null,
      };
    }

    function page6() {
      return {
        experienceAbroad: null,
        describeExperienceAbroad: null,
        pastAbuseHistory: null,
      };
    }
    
    return signupService;
  }
})();
