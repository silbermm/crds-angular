(function() {
  'use strict';

  module.exports = SignupStepController;

  SignupStepController.$inject = [
    '$stateParams',
    'TripsSignupService', '$scope'];

  function SignupStepController(
    $stateParams,
    TripsSignupService, $scope) {
    var vm = this;

    vm.signupService = $scope.$parent.tripsSignup.signupService;
    vm.step = $stateParams.stepId;

    vm.signupService.pageId = $stateParams.stepId;
    vm.signupService.pages[$stateParams.stepId] = {dirty: false};

  }
})();
