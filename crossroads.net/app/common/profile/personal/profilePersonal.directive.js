'use strict';
(function() {
  module.exports = ProfilePersonalDirective;

  ProfilePersonalDirective.$inject = ['$log', '$rootScope', 'ProfileReferenceData', 'Validation'];

  function ProfilePersonalDirective($log, $rootScope, ProfileReferenceData, Validation) {

    return {
      restrict: 'E',
      bindToController: true,
      scope: {
        updatedPerson: '=?',
        modalInstance: '=?',
        submitFormCallback: '&?',
        buttonText: '=',
        buttonCss: '@',
        allowPasswordChange: '=',
        requireMobilePhone: '=',
        forTrips: '=',
        profileData: '=?'
      },
      templateUrl: 'personal/profilePersonal.template.html',
      controller: 'ProfilePersonalController as profile',
    };
  }
})();
