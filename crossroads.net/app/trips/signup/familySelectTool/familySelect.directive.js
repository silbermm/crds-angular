(function() {
  'use strict';

  module.exports = FamilySelectDirective;

  FamilySelectDirective.$inject = ['TripsSignupService'];

  function FamilySelectDirective(TripsSignupService) {
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
      scope.isOfAge = isOfAge;
      scope.isSignedUp = isSignedUp;
      scope.signupService = TripsSignupService;
      scope.pClass = pClass;

      ////////////////////////////////
      //// Implementation Details ////
      ////////////////////////////////

      function divClass(member) {
        if (!member.signedUp && isOfAge(member)) {
          return 'col-sm-9 col-md-10';
        }
        return '';
      }

      function isOfAge(member) {
        if (member.age !== 0) {
          if (member.age < TripsSignupService.campaign.ageLimit) {
            if (_.includes(TripsSignupService.campaign.ageExceptions, Number(member.contactId))) {
              return true;
            }
            return false;
          }
        }
        return true;
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
