'use strict';
(function() {
  module.exports = ProfilePersonalDirective;

  ProfilePersonalDirective.$inject = ['$log', '$rootScope', 'ProfileReferenceData', 'Validation'];

  function ProfilePersonalDirective($log, $rootScope, ProfileReferenceData, Validation) {

    return {
      restrict: 'E',
      bindToController: true,
      scope: {
        allowPasswordChange: '=',
        buttonCss: '@',
        buttonText: '=',
        contactId: '=?',
        forTrips: '=',
        modalInstance: '=?',
        requireMobilePhone: '=',
        submitFormCallback: '&?',
        profileData: '=?',
        updatedPerson: '=?'
      },
      templateUrl: 'personal/profilePersonal.template.html',
      controller: 'ProfilePersonalController as profile',
    };
  }
})();
