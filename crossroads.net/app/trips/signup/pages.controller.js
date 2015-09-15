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
   *    - pageTitle
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
    vm.profileData = undefined;
    vm.underAge = underAge;
    vm.validation = Validation;
    vm.whyPlaceholder = '';
    vm.validateProfile = validateProfile;

    activate();

    function activate() {
      if (vm.pageTitle === 'Go India Application') {
        vm.whyPlaceholder = 'Please be specific. ' +
          'In instances where we have a limited number of spots, we strongly consider responses to this question.';
      }

      vm.signupService.activate();

    }

    function validateProfile(profile, household) {
      handleNext(2);
    }

    function handleNextt(nextPage, target) {
      var x = target;
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
      if (nextPage === 3) {
        angular.element('#tripAppPage2').trigger('submit');
      }

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
      var application = new vm.signupService.TripApplication();
      application.contactId = vm.signupService.contactId;
      application.pledgeCampaignId = vm.signupService.pledgeCampaignId;
      application.pageTwo = vm.signupService.page2;
      application.$save();

      // vm.signupService.TripApplication.$save();

      //below commented for testing
      // vm.currentPage = 'thanks';
      // toTop();
    }

    function nolaRequired() {
      if (vm.pageTitle === 'GO NOLA Application') {
        return 'required';
      }

      return '';
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
