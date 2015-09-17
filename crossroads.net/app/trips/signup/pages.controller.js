(function() {
  'use strict';

  module.exports = PagesController;

  PagesController.$inject = [
    '$rootScope',
    'Session',
    '$location',
    '$anchorScroll',
    '$log',
    'Trip',
    'Validation',
    'TripsSignupService'];

  /**
   * Controller for all of the pages directives
   * Variobles passed into the directives:
   *    - currentPage
   *    - destination
   *    - numberOfPages
   */
  function PagesController($rootScope, Session, $location, $anchorScroll, $log, Trip, Validation, TripsSignupService) {
    var vm = this;

    vm.buttonText = 'Next';
    vm.handleNext = handleNext;
    vm.handleNextt = handleNextt;
    vm.handlePrevious = handlePrevious;
    vm.handleSubmit = handleSubmit;
    vm.nolaRequired = nolaRequired;
    vm.signupService = TripsSignupService;
    vm.profileData = TripsSignupService.profileData;
    //vm.signupService.profileData = vm.profileData;
    vm.underAge = underAge;
    vm.validation = Validation;
    vm.whyPlaceholder = '';
    vm.validateProfile = validateProfile;

    activate();

    function activate() {
      if (vm.destination === 'India') {
        vm.whyPlaceholder = 'Please be specific. ' +
          'In instances where we have a limited number of spots, we strongly consider responses to this question.';
      }

      vm.signupService.activate();

    }

    function validateProfile(profile, household) {
      vm.signupService.page1 = {};
      vm.signupService.page1.profile = profile;
      vm.signupService.page1.household = household;

      handleNext(2);
    }

    function handleNextt(nextPage, target) {
      var form = target.tripAppPage2;
      form.$setSubmitted(true);

      if (form.$valid) {
        vm.currentPage = nextPage;
        toTop();
      } else {
        $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
      }
    }

    function handleNext(nextPage) {
      vm.currentPage = nextPage;
      toTop();
    }

    function handlePrevious(prevPage) {
      vm.currentPage = prevPage;
      toTop();
    }

    function handleSubmit() {
      // submit info and then show the thankyou page directive
      $log.debug(vm.signupService.page2);

      // vm.profileData.person.$save().success(function(data) {
      //   $log.debug('person save successful');
      // }).error(function(response, statusCode) {
      //   $log.debug('person save unsuccessful');
      // });

      vm.profileData.person.$save(function() {
        // $rootScope.$emit('notify', $rootScope.MESSAGES.profileUpdated);
        $log.debug('person save successful');
        // if (vm.modalInstance !== undefined) {
        //   vm.closeModal(true);
        // }
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

      vm.currentPage = 'thanks';
      toTop();
    }

    function nolaRequired() {
      if (vm.destination === 'NOLA') {
        return 'required';
      }

      return '';
    }

    function pageOne() {
      return {
        firstName: vm.signupService.page1.profile.person.firstName,
        lastName: vm.signupService.page1.profile.person.lastName
      };
    }

    function toTop() {
      $location.hash('form-top');
      $anchorScroll();
    }

    function underAge() {
      return Session.exists('age') && Session.exists('age') < 18;
    }
  }
})();
