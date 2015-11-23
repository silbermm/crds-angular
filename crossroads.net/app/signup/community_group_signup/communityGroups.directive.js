(function() {
  'use strict';

  module.exports = CommunityGroups;

  CommunityGroups.$inject = [];

  function CommunityGroups() {
    return {
      restrict: 'E',
      scope: {},
      templateUrl: 'community_group_signup/communityGroupSignupForm.html',
      controller: 'CommunityGroupSignupController as groupsignup',
      bindToController: true
    };

    function CommunityGroupController() {

    }
  }

})();
