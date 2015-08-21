(function() {
  'use strict';

  module.exports = FamilySelectDirective;

  FamilySelectDirective.$inject = [];

  function FamilySelectDirective() {
    return {
      restrict: 'E',
      replace: true,
      scope: {
        ageRestriction: '=',
        familyMembers: '=',
      },
      templateUrl: 'familySelectTool/familySelect.html',
    };
  }
})();
