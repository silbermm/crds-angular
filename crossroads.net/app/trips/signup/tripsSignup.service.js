(function() {
  'use strict';

  module.exports = TripsSignupService;

  TripsSignupService.$inject = ['$resource', '$location', '$log', 'Session'];

  function TripsSignupService($resource, $location, $log, Session) {
    var signupService = {
      activate: activate,
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
    }

    function page2() {
      return {
        guardianFirstName: {formFieldId: 1221, value: null},
        guardianLastName: {formFieldId: 1222, value: null},
        tshirtSize: null,
        scrubSizeBottom: null,
        scrubSizeTop: null,
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
    }

    function page3() {
      return {
        emergencyContactFirstName: {formFieldId: 1235, value: null},
        emergencyContactLastName: {formFieldId: 1236, value: null},
        emergencyContactEmail: {formFieldId: 1237, value: null},
        emergencyContactPrimaryPhone: {formFieldId: 1238, value: null},
        emergencyContactSecondaryPhone: {formFieldId: 1239, value: null}
      };
    }

    function page4() {
      return {
        lottery: {formFieldId: 1240, value: null},
        groupCommonName: {formFieldId: 1241, value: null},
        roommateFirstChoice: {formFieldId: 1242, value: null},
        roommateSecondChoice: {formFieldId: 1243, value: null},
        supportPersonEmail: {formFieldId: 1244, value: null},
        interestedInGroupLeader: {formFieldId: 1245, value: null},
        whyGroupLeader: {formFieldId: 1246, value: null},
      };
    }

    function page5() {
      return {
        sponsorChildInNicaragua: {formFieldId: 1150, value: null},
        sponsorChildFirstName: {formFieldId: 1151, value: null},
        sponsorChildLastName: {formFieldId: 1152, value: null},
        sponsorChildNumber: {formFieldId: 1153, value: null},
        nolaFirstChoiceWorkTeam: {formFieldId: 1205, value: null},
        nolaFirstChoiceExperience: {formFieldId: 1206, value: null},
        nolaSecondChoiceWorkTeam: {formFieldId: 1207, value: null},
        previousTripExperience: {formFieldId: 1247, value: null},
        professionalSkillBusiness: {formFieldId: 1248, value: null},
        professionalSkillConstruction: {formFieldId: 1249, value: null},
        professionalSkillDental: {formFieldId: 1250, value: null},
        professionalSkillEducation: {formFieldId: 1251, value: null},
        professionalSkillInformationTech: {formFieldId: 1252, value: null},
        professionalSkillMedia: {formFieldId: 1253, value: null},
        professionalSkillMedical: {formFieldId: 1254, value: null},
        professionalSkillMusic: {formFieldId: 1255, value: null},
        professionalSkillOther: {formFieldId: 1256, value: null},
        professionalSkillPhotography: {formFieldId: 1257, value: null},
        professionalSkillSocialWorker: {formFieldId: 1258, value: null},
        professionalSkillStudent: {formFieldId: 1259, value: null}
      };
    }

    function page6() {
      return {
        validPassport: {formFieldId: 1260, value: null},
        passportExpirationDate: {formFieldId: 1261, value: null},
        passportFirstName: {formFieldId: 1262, value: null},
        passportMiddleName: {formFieldId: 1263, value: null},
        passportLastName: {formFieldId: 1264, value: null},
        passportCountry: {formFieldId: 1265, value: null},
        passportBirthday: {formFieldId: 1266, value: null},
        deltaFrequentFlyer: {formFieldId: 1267, value: null},
        southAfricanFrequentFlyer: {formFieldId: 1268, value: null},
        unitedFrequentFlyer: {formFieldId: 1269, value: null},
        usAirwaysFrequentFlyer: {formFieldId: 1176, value: null},
        internationalTravelExpericence: {formFieldId: 1270, value: null},
        experienceAbroad: {formFieldId: 1271, value: null},
        describeExperienceAbroad: {formFieldId: 1272, value: null},
        pastAbuseHistory: {formFieldId: 1124, value: null},
      };
    }

    function saveApplication() {
      $log.debug(signupService.page2);
    }

    return signupService;
  }
})();
