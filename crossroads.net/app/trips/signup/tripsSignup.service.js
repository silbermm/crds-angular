var attributes = require('crds-constants').ATTRIBUTE_IDS;
var attributeTypes = require('crds-constants').ATTRIBUTE_TYPE_IDS;
(function() {
  'use strict';

  module.exports = TripsSignupService;

  TripsSignupService.$inject = ['$resource', '$location', '$log', 'Session'];

  function TripsSignupService($resource, $location, $log, Session) {
    var signupService = {
      activate: activate,
      evaluateTripSkills: evaluateTripSkills,
      pages: [],
      reset: reset,
      TripApplication: $resource(__API_ENDPOINT__ + 'api/trip-application'),
      thankYouMessage: '',
      tshirtSizes: tshirtSizes(),
      topScrubSizes: topScrubSizes(),
      bottomScrubSizes: bottomScrubSizes(),
      frmPage2: {}
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

    function evaluateTripSkills(tripSkills) {
          }

    function bottomScrubSizes() {
      return [{
        formFieldId: 1429,
        attributeId: 3174,
        value: 'Adult XS'
      }, {
        formFieldId: 1429,
        attributeId: 3175,
        value: 'Adult S'
      }, {
        formFieldId: 1429,
        attributeId: 3176,
        value: 'Adult M'
      }, {
        formFieldId: 1429,
        attributeId: 3177,
        value: 'Adult L'
      }, {
        formFieldId: 1429,
        attributeId: 3178,
        value: 'Adult XL'
      }, {
        formFieldId: 1429,
        attributeId: 3179,
        value: 'Adult XXL'
      }, {
        formFieldId: 1429,
        attributeId: 3180,
        value: 'Adult XXXL'
      }];
    }

    function topScrubSizes() {
      return [{
        formFieldId: 1477,
        attributeId: 3167,
        value: 'Adult XS'
      }, {
        formFieldId: 1477,
        attributeId: 3168,
        value: 'Adult S'
      }, {
        formFieldId: 1477,
        attributeId: 3169,
        value: 'Adult M'
      }, {
        formFieldId: 1477,
        attributeId: 3170,
        value: 'Adult L'
      }, {
        formFieldId: 1477,
        attributeId: 3171,
        value: 'Adult XL'
      }, {
        formFieldId: 1477,
        attributeId: 3172,
        value: 'Adult XXL'
      }, {
        formFieldId: 1477,
        attributeId: 3173,
        value: 'Adult XXXL'
      }];
    }

    function tshirtSizes() {
      return ['Adult XS',
      'Adult S',
      'Adult M',
      'Adult L',
      'Adult XL',
      'Adult XXL',
      'Adult XXXL',
      'Child S',
      'Child M',
      'Child L'];
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
        tshirtSize: null,
        scrubSizeBottom: null,
        scrubSizeTop: null,
        referral: null,
        conditions: null,
        vegetarian: null,
        allergies: null,
        why: null,
        spiritualLife: null,
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
        nolaFirstChoiceWorkTeam: null,
        nolaFirstChoiceExperience: null,
        nolaSecondChoiceWorkTeam: null,
        previousTripExperience: null,
        professionalSkills: null,
      };
    }

    function page6() {
      return {
        validPassport: null,
        passportExpirationDate: null,
        passportFirstName: null,
        passportMiddleName: null,
        passportLastName: null,
        passportCountry: null,
        passportNumber: null,
        deltaFrequentFlyer: null,
        southAfricanFrequentFlyer: null,
        unitedFrequentFlyer: null,
        usAirwaysFrequentFlyer: null,
        internationalTravelExpericence: null,
        experienceAbroad: null,
        describeExperienceAbroad: null,
        pastAbuseHistory: null,
      };
    }

    return signupService;
  }
})();
