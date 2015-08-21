(function() {
  'use strict';

  module.exports = ThankYouPageDirective;

  ThankYouPageDirective.$inject = [];

  function ThankYouPageDirective() {
    return {
      restrict: 'E',
      replace: true,
      scope: {
        currentPage: '=',
        pageTitle: '=',
        numberOfPages: '=',
      },
      templateUrl: 'thankyou/thankYou.html',
      controller: 'PagesController as pages',
      bindToController: true,
    };

  }
})();
