(function() {
  'use strict';

  module.exports = SignupPage4Directive;

  SignupPage4Directive.$inject = [];

  function SignupPage4Directive() {
    return {
      restrict: 'E',
      replace: true,
      scope: {
        currentPage: '=',
        pageTitle: '=',
      },
      controller: 'PagesController as pages',
      bindToController: true,
      templateUrl: 'page4/signupPage4.html',
    };
  }

})();
