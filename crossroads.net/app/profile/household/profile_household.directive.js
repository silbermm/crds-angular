'use strict';
(function() {
  module.exports = ProfileHouseholdDirective;

  ProfileHouseholdDirective.$inject = ['$log'];

  function ProfileHouseholdDirective($log) {
    $log.debug('ProfileHouseholdDirective');
    return {
      restrict: 'E',
      transclude: true,
      // bindToController: true,
      scope: {
        householdId: '='
      },
      templateUrl: 'household/profile_household.template.html',
      // controller: 'ProfileHouseholdController as household',
      link: link
    };
  }

  function link(scope, el, attr, controller) {
    var throwAway = scope.householdId;
  }

})();
