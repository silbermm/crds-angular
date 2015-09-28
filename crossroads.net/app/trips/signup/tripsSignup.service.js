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
      return [{
        formFieldId: 1428,
        attributeId: 3157,
        value: 'Adult XS'
      }, {
        formFieldId: 1428,
        attributeId: 3158,
        value: 'Adult S'
      }, {
        formFieldId: 1428,
        attributeId: 3159,
        value: 'Adult M'
      }, {
        formFieldId: 1428,
        attributeId: 3160,
        value: 'Adult L'
      }, {
        formFieldId: 1428,
        attributeId: 3161,
        value: 'Adult XL'
      }, {
        formFieldId: 1428,
        attributeId: 3162,
        value: 'Adult XXL'
      }, {
        formFieldId: 1428,
        attributeId: 3163,
        value: 'Adult XXXL'
      }, {
        formFieldId: 1428,
        attributeId: 3164,
        value: 'Child S'
      }, {
        formFieldId: 1428,
        attributeId: 3165,
        value: 'Child M'
      }, {
        formFieldId: 1428,
        attributeId: 3166,
        value: 'Child L'
      }];
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
        guardianFirstName: {formFieldId: 1426, value: null},
        guardianLastName: {formFieldId: 1427, value: null},
        tshirtSize: null,
        scrubSizeBottom: null,
        scrubSizeTop: null,
        referral: {formFieldId: 1433, value: null},
        conditions: {formFieldId: 1432, value: null},
        vegetarian: {formFieldId: 1430, value: null},
        allergies: {formFieldId: 1431, value: null},
        spiritualLifeSearching: {formFieldId: 1435, value: null},
        spiritualLifeReceived: {formFieldId: 1436, value: null},
        spiritualLifeObedience: {formFieldId: 1437, value: null},
        spiritualLifeReplicating: {formFieldId: 1438, value: null},
        why: {formFieldId: 1434, value: null}
      };
    }

    function page3() {
      return {
        emergencyContactFirstName: {formFieldId: 1439, value: null},
        emergencyContactLastName: {formFieldId: 1440, value: null},
        emergencyContactEmail: {formFieldId: 1441, value: null},
        emergencyContactPrimaryPhone: {formFieldId: 1442, value: null},
        emergencyContactSecondaryPhone: {formFieldId: 1443, value: null}
      };
    }

    function page4() {
      return {
        lottery: {formFieldId: 1444, value: null},
        groupCommonName: {formFieldId: 1445, value: null},
        roommateFirstChoice: {formFieldId: 1446, value: null},
        roommateSecondChoice: {formFieldId: 1447, value: null},
        supportPersonEmail: {formFieldId: 1488, value: null},
        interestedInGroupLeader: {formFieldId: 1449, value: null},
        whyGroupLeader: {formFieldId: 1450, value: null},
      };
    }

    function page5() {
      return {
        sponsorChildInNicaragua: {formFieldId: 1418, value: null},
        sponsorChildFirstName: {formFieldId: 1419, value: null},
        sponsorChildLastName: {formFieldId: 1420, value: null},
        sponsorChildNumber: {formFieldId: 1421, value: null},
        nolaFirstChoiceWorkTeam: {formFieldId: 1423, value: null},
        nolaFirstChoiceExperience: {formFieldId: 1424, value: null},
        nolaSecondChoiceWorkTeam: {formFieldId: 1207, value: null},
        previousTripExperience: {formFieldId: 1451, value: null},
        professionalSkillBusiness: {formFieldId: 1452, value: null},
        professionalSkillConstruction: {formFieldId: 1453, value: null},
        professionalSkillDental: {formFieldId: 1454, value: null},
        professionalSkillEducation: {formFieldId: 1455, value: null},
        professionalSkillInformationTech: {formFieldId: 1456, value: null},
        professionalSkillMedia: {formFieldId: 1457, value: null},
        professionalSkillMedical: {formFieldId: 1458, value: null},
        professionalSkillMusic: {formFieldId: 1459, value: null},
        professionalSkillOther: {formFieldId: 1460, value: null},
        professionalSkillPhotography: {formFieldId: 1461, value: null},
        professionalSkillSocialWorker: {formFieldId: 1462, value: null},
        professionalSkillStudent: {formFieldId: 1463, value: null}
      };
    }

    function page6() {
      return {
        validPassport: {formFieldId: 1464, value: null},
        passportExpirationDate: {formFieldId: 1465, value: null},
        passportFirstName: {formFieldId: 1466, value: null},
        passportMiddleName: {formFieldId: 1477, value: null},
        passportLastName: {formFieldId: 1468, value: null},
        passportCountry: {formFieldId: 1469, value: null},
        passportBirthday: {formFieldId: 1470, value: null},
        deltaFrequentFlyer: {formFieldId: 1471, value: null},
        southAfricanFrequentFlyer: {formFieldId: 1472, value: null},
        unitedFrequentFlyer: {formFieldId: 1473, value: null},
        usAirwaysFrequentFlyer: {formFieldId: 1422, value: null},
        internationalTravelExpericence: {formFieldId: 1474, value: null},
        experienceAbroad: {formFieldId: 1475, value: null},
        describeExperienceAbroad: {formFieldId: 1476, value: null},
        pastAbuseHistory: {formFieldId: 1417, value: null},
      };
    }

    function saveApplication() {
      $log.debug(signupService.page2);
    }

    return signupService;
  }
})();
