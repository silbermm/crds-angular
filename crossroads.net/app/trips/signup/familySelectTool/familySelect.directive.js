(function() {
  'use strict';

  module.exports = FamilySelectDirective;

  FamilySelectDirective.$inject = ['$state', 'TripsSignupService'];

  function FamilySelectDirective($state, TripsSignupService) {
    return {
      restrict: 'E',
      replace: true,
      scope: {
        ageRestriction: '=',
        familyMembers: '=',
      },
      templateUrl: 'familySelectTool/familySelect.html',
      link: link
    };

    function link(scope, el, attr) {
      scope.divClass = divClass;
      scope.goToApp = goToApp;
      scope.isSignedUp = isSignedUp;
      scope.signupService = TripsSignupService;
      scope.pClass = pClass;

      ////////////////////////////////
      //// Implementation Details ////
      ////////////////////////////////

      function divClass(member) {
        if (!member.signedUp) {
          return 'col-sm-9 col-md-10';
        }

        return '';
      }

      function goToApp(contactId) {
        scope.signupService.contactId = contactId;
        // $state.go(tripsignup.application({campaignId: signupService.campaign.id }));
        $state.go('tripsignup.application', {campaignId: scope.signupService.campaign.id});
      }

      function isSignedUp(member) {
        return member.signedUp;
      }

      function pClass(member) {
        if(!member.signedUp) {
          return 'flush-bottom';
        }

        return '';
      }

    }

  }

})();
