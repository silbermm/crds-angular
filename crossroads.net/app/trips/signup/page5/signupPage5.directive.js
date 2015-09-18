(function() {
  'use strict';

  module.exports = SignupPage3Directive;

  SignupPage3Directive.$inject = [];

  function SignupPage3Directive() {
    return {
      restrict: 'E',
      replace: true,
      scope: {
        currentPage: '=',
        destination: '=',
        numberOfPages: '=',
        workTeams: '=',
      },
      templateUrl: 'page5/signupPage5.html',
      controller: 'PagesController as pages',
      bindToController: true,
    };

  }
})();
