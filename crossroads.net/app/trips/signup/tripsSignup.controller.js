(function() {
  'use strict';

  module.exports = TripsSignupController;

  TripsSignupController.$inject = ['$log',
    '$rootScope',
    'Session',
    'Campaign',
    'WorkTeams',
    '$location',
    'Trip',
    '$q',
    'contactId',
    'TripsSignupService',
    'Person'
  ];

  function TripsSignupController(
      $log,
      $rootScope,
      Session,
      Campaign,
      WorkTeams,
      $location,
      Trip,
      $q,
      contactId,
      TripsSignupService,
      Person ){

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
    vm.viewReady = false;
    vm.whyPlaceholder = '';
    vm.workTeams = WorkTeams;

    activate();

    ////////////////////////////////
    //// IMPLEMENTATION DETAILS ////
    ////////////////////////////////
    function activate() {
      TripsSignupService.profileData = { person:  Person };
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

      }, function(reason) {
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
            }, function(error) {
              resolve(true);
            });
          }
        }
      });
    }
  }

})();
