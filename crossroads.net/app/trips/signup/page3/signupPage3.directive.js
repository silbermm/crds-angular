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
      },
      templateUrl: 'page3/signupPage3.html',
      controller: 'PagesController as pages',
      bindToController: true,
    };

  }
})();
