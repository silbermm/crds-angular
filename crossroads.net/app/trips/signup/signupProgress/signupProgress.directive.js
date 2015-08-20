(function() {
  'use strict';

  module.exports = SignupDirective;

  SignupDirective.$inject = [];

  function SignupDirective() {
    return {
      restrict: 'E',
      scope: {
        currentStep: '=',
        totalSteps: '=',
      },
      templateUrl: 'signupProgress/signupProgress.html',
      controller: 'SignupProgressController as progress',
      bindToController: true,
    };
  }

})();
