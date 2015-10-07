(function() {
  'use strict()';

  module.exports = HouseholdInformation;

  HouseholdInformation.$inject = ['$log'];

  function HouseholdInformation($log) {
    return {
      restrict: 'EA',
      transclude: true,
      templateUrl: 'templates/household_information.html',
      scope: {
        profile: '='
      },
      link: link
    };

    function link(scope, el, attr) {
      /* Nothing - leaving for future expandability */
    }
  }
})();