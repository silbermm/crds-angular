(function() {
  'use strict';

  module.exports = SignupStepController;

  SignupStepController.$inject = [
    '$stateParams',
    'TripsSignupService',
    '$scope',

    // For the dropdowns 
    'WorkTeams',
    'ScrubTopSizes',
    'ScrubBottomSizes',
    'TshirtSizes',
    'SpiritualJourney',
    'TripSkills',
    'FrequentFlyers',

  ];

  function SignupStepController(
    $stateParams,
    TripsSignupService,
    $scope,
    WorkTeams,
    ScrubTopSizes,
    ScrubBottomSizes,
    TshirtSizes,
    SpiritualJourney,
    TripSkills,
    FrequentFlyers) {

    var vm = this;

    vm.scrubBottomSizes = ScrubBottomSizes;
    vm.scrubTopSizes = ScrubTopSizes;
    vm.signupService = $scope.$parent.tripsSignup.signupService;
    vm.step = $stateParams.stepId;
    vm.workTeams = WorkTeams;

    vm.signupService.pageId = $stateParams.stepId;
    vm.signupService.pages[$stateParams.stepId] = {dirty: false};

  }
})();
