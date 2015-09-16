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
      controller: 'PagesController as pages',
      bindToController: true,
      templateUrl: 'page6/signupPage6.html',
    };

  }
})();
