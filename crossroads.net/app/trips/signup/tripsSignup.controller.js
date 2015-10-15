(function() {
  'use strict';

  module.exports = TripsSignupController;

  TripsSignupController.$inject = [
    '$log',
    '$rootScope',
    '$scope',
    '$state',
    'Session',
    'Campaign',
    'WorkTeams',
    '$location',
    'Trip',
    '$q',
    'contactId',
    'TripsSignupService',
    'Person',
    'Validation',
    '$window',
    '$anchorScroll',
    '$stateParams'
  ];

  function TripsSignupController(
      $log,
      $rootScope,
      $scope,
      $state,
      Session,
      Campaign,
      WorkTeams,
      $location,
      Trip,
      $q,
      contactId,
      TripsSignupService,
      Person,
      Validation,
      $window,
      $anchorScroll,
      $stateParams
    )
    {

    var vm = this;
    vm.ageLimitReached = true;
    vm.buttonText = 'Next';
    vm.campaign = Campaign;
    vm.commonNameRequired = commonNameRequired;
    vm.contactId = contactId;
    vm.destination = vm.campaign.nickname;
    vm.handlePageChange = handlePageChange;
    vm.handleSubmit = handleSubmit;
    vm.hasPassport = hasPassport;
    vm.isIndia = isIndia;
    vm.isNica = isNica;
    vm.isNola = isNola;
    vm.isSouthAfrica = isSouthAfrica;
    vm.numberOfPages = 0;
    vm.pageHasErrors = true;
    vm.privateInvite = $location.search()['invite'];
    vm.profileData = {};
    vm.progressLabel = '';
    vm.registrationNotOpen = true;
    vm.requireInternational = requireInternational;
    vm.signupService = TripsSignupService;
    vm.skillsSelected = skillsSelected;
    vm.spiritualSelected = spiritualSelected;
    vm.tripName = vm.campaign.name;
    vm.underAge = underAge;
    vm.validateProfile = validateProfile;
    vm.validation = Validation;
    vm.phoneFormat = vm.validation.phoneFormat();
    vm.viewReady = false;
    vm.whyPlaceholder = '';
    vm.workTeams = WorkTeams;

    $rootScope.$on('$stateChangeStart', stateChangeStart);
    $scope.$on('$viewContentLoaded', stateChangeSuccess);
    $window.onbeforeunload = onBeforeUnload;

    activate();

    ////////////////////////////////
    //// IMPLEMENTATION DETAILS ////
    ////////////////////////////////
    function activate() {

      if (vm.signupService.campaign === undefined) {
        vm.signupService.reset(vm.campaign);
      }

      vm.signupService.pageId = 1;

      if (vm.destination === 'India') {
        vm.whyPlaceholder = 'Please be specific. ' +
          'In instances where we have a limited number of spots, we strongly consider responses to this question.';
      }

      vm.signupService.activate();

      TripsSignupService.profileData = { person:  Person };
      vm.profileData = TripsSignupService.profileData;
      vm.progressLabel = progressLabel();
      if (TripsSignupService.campaign === undefined) {
        TripsSignupService.campaign = Campaign;
      }

      pageHasErrors();

      switch (vm.destination) {
        case 'NOLA':
          vm.numberOfPages = 5;
          TripsSignupService.thankYouMessage = $rootScope.MESSAGES.NOLASignUpThankYou.content;
          break;
        case 'South Africa':
          vm.numberOfPages = 6;
          TripsSignupService.thankYouMessage = $rootScope.MESSAGES.SouthAfricaSignUpThankYou.content;
          break;
        case 'India':
          vm.numberOfPages = 6;
          vm.whyPlaceholder = 'Please be specific. ' +
            'In instances where we have a limited number of spots, we strongly consider responses to this question.';
          TripsSignupService.thankYouMessage = $rootScope.MESSAGES.IndiaSignUpThankYou.content;
          break;
        case 'Nicaragua':
          vm.numberOfPages = 6;
          TripsSignupService.thankYouMessage = $rootScope.MESSAGES.NicaraguaSignUpThankYou.content;
          break;
      }

      toTop();
    }

    function commonNameRequired() {
      switch (vm.signupService.page4.lottery.value) {
        case null:
          return false;
        case 'As long as I am selected, I will go on the trip.':
          return false;
        default:
          return true;
      }
    }

    function handlePageChange(pageId, form) {
      var route;
      if (form !== null) {
        form.$setSubmitted(true);
        if (form.$valid) {
          $log.debug('form valid');
          route = 'tripsignup.application.page';
          $state.go(route, {stepId: pageId});
        } else {
          $log.debug('form INVALID');
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
          return false;
        }
      } else {
        //find a way to make this more generic
        route = 'tripsignup.application.page';
        $state.go(route, {stepId: pageId});
      }
    }

    function handleSubmit(form) {
      $log.debug('handleSubmit start');
      if (form !== null) {
        form.$setSubmitted(true);
        if (form.$valid) {
          $log.debug('form valid');
          saveData();
        } else {
          $log.debug('form INVALID');
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
        }
      } else {
        saveData();
      }
    }

    function onBeforeUnload() {
      $log.debug('onBeforeUnload start');
      if (vm.tpForm.$dirty) {
        return '';
      }
    }

    function isIndia() {
      if (vm.destination === 'India') {
        return true;
      }

      return false;
    }

    function isNica() {
      if (vm.destination === 'Nicaragua') {
        return true;
      }

      return false;
    }

    function isNola() {
      if (vm.destination === 'NOLA') {
        return true;
      }

      return false;
    }

    function isSouthAfrica() {
      if (vm.destination === 'South Africa') {
        return true;
      }

      return false;
    }

    function preliminaryAgeCheck() {
      var age = Session.exists('age');
      if (age === '0') {
        // null value for birth date is converted to age = 0
        // validate age based on required field birth date on pg 1 submit
        return false;
      }

      if (age === undefined) {
        // age is undefned
        // validate age based on required field birth date on pg 1 submit
        return false;
      }

      if (age < Campaign.ageLimit) {
        //Under age limit, check for exceptions
        var userId = Session.exists('userId');
        if (userId && _.includes(Campaign.ageExceptions, Number(userId))) {
          return false;
        }

        return true;
      }

      return false;
    }

    function pageHasErrors() {
      vm.ageLimitReached = preliminaryAgeCheck();
      var promise = registrationNotOpen();
      promise.then(function(regNotOpen) {
        vm.registrationNotOpen = regNotOpen;
        if (vm.ageLimitReached || vm.registrationNotOpen) {
          vm.pageHasErrors = true;
        } else {
          vm.pageHasErrors = false;
        }

        vm.viewReady = true;

      },

      function(reason) {
        vm.pageHasErrors = true;
        vm.viewReady = true;
      });
    }

    function hasPassport() {
      return (vm.signupService.page6.validPassport.value === 'yes');
    }

    function progressLabel() {
      return vm.profileData.person.nickName + ' ' + vm.profileData.person.lastName;
    }

    function registrationNotOpen() {
      return $q(function(resolve, reject) {
        var regStart = moment(vm.campaign.registrationStart);
        var regEnd = moment(vm.campaign.registrationEnd);
        var today = moment();
        if (today.isBetween(regStart, regEnd)) {
          resolve(false);
        } else {
          if (vm.privateInvite === undefined) {
            resolve(true);
          } else {
            Trip.ValidatePrivateInvite.get({
              pledgeCampaignId: vm.campaign.id,
              guid: vm.privateInvite
            }, function(data) {
              resolve(!data.valid);
            },

            function(error) {
              resolve(true);
            });
          }
        }
      });
    }

    function requireInternational() {
      if (vm.destination === 'NOLA') {
        return false;
      }

      return true;
    }

    function saveData() {
      vm.profileData.person.$save(function() {
        $log.debug('person save successful');
      }, function() {

        $log.debug('person save unsuccessful');
      });

      var application = new vm.signupService.TripApplication();
      application.contactId = vm.signupService.contactId;
      application.pledgeCampaignId = vm.signupService.campaign.id;
      application.pageTwo = vm.signupService.page2;
      application.pageThree = vm.signupService.page3;
      application.pageFour = vm.signupService.page4;
      application.pageFive = vm.signupService.page5;
      application.pageSix = vm.signupService.page6;
      application.inviteGUID = $stateParams.invite;
      application.$save(function() {
        $log.debug('trip application save successful');
      }, function() {

        $log.debug('trip application save unsuccessful');
      });

      _.each(vm.signupService.familyMembers, function(f) {
        if (f.contactId === vm.signupService.contactId) {
          f.signedUp = true;
          f.signedUpDate = new Date();
        }
      });

      vm.signupService.pageId = 'thanks';
      vm.tpForm.$setPristine();
      $state.go('tripsignup.application.thankyou');
    }

    function skillsSelected() {
      if (vm.signupService.page5.professionalSkillBusiness.value ||
          vm.signupService.page5.professionalSkillConstruction.value ||
          vm.signupService.page5.professionalSkillDental.value ||
          vm.signupService.page5.professionalSkillEducation.value ||
          vm.signupService.page5.professionalSkillInformationTech.value ||
          vm.signupService.page5.professionalSkillMedia.value ||
          vm.signupService.page5.professionalSkillMedical.value ||
          vm.signupService.page5.professionalSkillMusic.value ||
          vm.signupService.page5.professionalSkillPhotography.value ||
          vm.signupService.page5.professionalSkillSocialWorker.value ||
          vm.signupService.page5.professionalSkillStudent.value ||
          vm.signupService.page5.professionalSkillOther.value)
      {
        return true;
      }

      return false;
    }

    function spiritualSelected() {
      if (vm.signupService.page2.spiritualLifeSearching.value ||
          vm.signupService.page2.spiritualLifeReceived.value ||
          vm.signupService.page2.spiritualLifeObedience.value ||
          vm.signupService.page2.spiritualLifeReplicating.value) {
        return true;
      }

      return false;
    }

    function stateChangeStart(event, toState, toParams, fromState, fromParams) {
      if (fromState.name === 'tripsignup.application.thankyou') {
        if (toState.name === 'tripsignup.application.page') {
          event.preventDefault();
          $state.go('tripsignup', {campaignId: toParams.campaignId});
        }

        return;
      }

      if (toState.name === 'tripsignup.application.thankyou') {
        if (fromState.name !== 'tripsignup.application.page') {
          event.preventDefault();
          $state.go('tripsignup', {campaignId: toParams.campaignId});
        }
      }

      if (toState.name === 'tripsignup') {
        // warn if the form is dirty
        if (vm.tpForm) {
          if (vm.tpForm.$dirty) {
            if (!$window.confirm('Are you sure you want to leave this page?')) {
              event.preventDefault();
              return;
            }
          }
        }

        // reset service on "page 0"
        vm.signupService.reset(vm.campaign);
        return;
      }
    }

    function stateChangeSuccess(event) {
      toTop();
    }

    function toTop() {
      $location.hash('form-top');
      $anchorScroll();
    }

    function underAge() {
      return Session.exists('age') && Session.exists('age') < 18;
    }

    function validateProfile(profile, household) {
      vm.signupService.page1 = {};
      vm.signupService.page1.profile = profile;
      vm.signupService.page1.household = household;

      handlePageChange(2, null);
    }
  }

})();
