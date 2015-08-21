(function() {
  'use strict';

  module.exports = SignupPage2Directive;

  SignupPage2Directive.$inject = [];

  function SignupPage2Directive() {
    return {
      restrict: 'E',
      replace: true,
      scope: {
        currentPage: '=',
        pageTitle: '=',
        numberOfPages: '=',
      },
      templateUrl: 'page2/signupPage2.html',
      controller: 'PagesController as pages',
      bindToController: true,
    };

  }
})();
