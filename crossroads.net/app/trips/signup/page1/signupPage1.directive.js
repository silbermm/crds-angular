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
        destination: '=',
        numberOfPages: '=',
      },
      controller: 'PagesController as pages',
      bindToController: true,
      templateUrl: 'page1/signupPage1.html',
      link: link
    };

    function link(scope, el, attr, vm) {

    }

  }
})();
