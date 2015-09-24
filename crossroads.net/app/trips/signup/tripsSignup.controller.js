(function() {
  'use strict';

  module.exports = TripsSignupController;

  TripsSignupController.$inject = [
    '$log',
    '$rootScope',
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
    'Validation', '$window'
  ];

  function TripsSignupController(
      $log,
      $rootScope,
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
      Validation, $window
    )
    {

    var vm = this;
    vm.ageLimitReached = true;
    vm.campaign = Campaign;
    vm.contactId = contactId;
    vm.currentPage = 1;
    vm.destination = vm.campaign.nickname;
    vm.numberOfPages = 0;
    vm.pageHasErrors = true;
    vm.privateInvite = $location.search()['invite'];
    vm.registrationNotOpen = true;
    vm.tripName = vm.campaign.name;
    vm.tshirtSizes = [];
    vm.viewReady = false;
    vm.whyPlaceholder = '';
    vm.workTeams = WorkTeams;

    // from pages.controller
    ////////////////////////
    vm.buttonText = 'Next';
    // vm.handleNext = handleNext;
    // vm.handleNextt = handleNextt;
    vm.handlePageChange = handlePageChange;
    // vm.handlePrevious = handlePrevious;
    vm.handleSubmit = handleSubmit;
    vm.nolaRequired = nolaRequired;
    vm.signupService = TripsSignupService;
    // vm.profileData = TripsSignupService.profileData;
    vm.profileData = {};
    vm.underAge = underAge;
    vm.validation = Validation;
    vm.whyPlaceholder = '';
    vm.validateProfile = validateProfile;
    // from pages.controler end
    ////////////////////////

    $rootScope.$on('$stateChangeStart', stateChangeStart);
    // $window.onbeforeunload = onBeforeUnload;

    activate();

    ////////////////////////////////
    //// IMPLEMENTATION DETAILS ////
    ////////////////////////////////
    function activate() {

      if (vm.signupService.campaign === undefined) {
        vm.signupService.reset(vm.campaign);
      }

      // from pages.controller
      ////////////////////////
      if (vm.destination === 'India') {
        vm.whyPlaceholder = 'Please be specific. ' +
          'In instances where we have a limited number of spots, we strongly consider responses to this question.';
      }

      vm.signupService.activate();
      // vm.tshirtSizes = vm.signupService.tshirtSizes;

      // from pages.controller end
      ////////////////////////////

      TripsSignupService.profileData = { person:  Person };
      vm.profileData = TripsSignupService.profileData;
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
    }

    //this may be the way we handle validation in the next story
    // function handleNextt(nextPage, target) {
    //   var form = target.tripAppPage2;
    //   form.$setSubmitted(true);
    //
    //   if (form.$valid) {
    //     vm.currentPage = nextPage;
    //     toTop();
    //   } else {
    //     $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
    //   }
    // }

    function handlePageChange(pageId) {
      var route = 'tripsignup.application.page' + pageId;
      $state.go(route);
    }

    // function handleNext(nextPage) {
    //   var route = 'tripsignup.application.page' + nextPage;
    //   // var route = 'tripsignup.application.page2';
    //   // $state.go(route, {campaignId: vm.signupService.campaign.id, contactId: vm.profileData.person.contactId });
    //   $state.go(route);
    //   // vm.currentPage = nextPage;
    //   // toTop();
    // }

    // function handlePrevious(prevPage) {
    //   vm.currentPage = prevPage;
    //   toTop();
    // }

    function handleSubmit() {
      $log.debug('handleSubmit start');

      // submit info and then show the thankyou page directive
      //$log.debug(vm.signupService.page2);

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

      $log.debug('go thankyou');
      $state.go('tripsignup.application.thankyou');

      // vm.currentPage = 'thanks';
      // toTop();
    }

    function nolaRequired() {
      if (vm.destination === 'NOLA') {
        return 'required';
      }

      return '';
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

    function stateChangeStart(event, toState, toParams, fromState, fromParams) {
      // if ($scope.serveForm !== undefined) {
      //   checkChildForms();
      //   if ($scope['serveForm'].$dirty) {
      //     if(!$window.confirm('Are you sure you want to leave this page?')) {
      //       event.preventDefault();
      //       return;
      //     }
      //   }
      // }
      if (!$window.confirm('Are you sure you want to leave this page?')) {
        event.preventDefault();
        return;
      }
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

      // handleNext(2);
      handlePageChange(2);
    }
  }

})();
