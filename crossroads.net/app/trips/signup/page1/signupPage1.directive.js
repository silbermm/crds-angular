(function() {
  'use strict';

  module.exports = SignupPage1Directive;

  SignupPage1Directive.$inject = [];

  function SignupPage1Directive() {
    return {
      restrict: 'E',
      replace: true,
      scope: {
        currentPage: '=',
        pageTitle: '=',
        numberOfPages: '=',
      },
      controller: 'PagesController as pages',
      bindToController: true,
      templateUrl: 'page1/signupPage1.html',
    };

  }
})();
