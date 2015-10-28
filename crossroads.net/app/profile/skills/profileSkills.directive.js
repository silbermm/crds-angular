(function() {
  'use strict';

  module.exports = ProfileSkills;

  function ProfileSkills() {
    return {
      restrict: 'E',
      replace: true,
      scope: {},
      templateUrl: 'skills/profileSkills.html',
      controller: 'ProfileSkillsController as profileSkills',
      bindToController: true
    };
  }

})();
