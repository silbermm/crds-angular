(function() {
  'use strict()';

  module.exports = GivingYears;

  GivingYears.$inject = ['$log'];

  function GivingYears($log) {
    return {
      restrict: 'EA',
      transclude: true,
      templateUrl: 'templates/giving_years.html',
      scope: {
        selectedYear: '=',
        allYears: '=',
        onChange: '&'
      },
      link: link
    };

    function link(scope, el, attr) {
      /* Nothing - leaving for future expandability */
    }
  }
})();