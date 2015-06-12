"use strict()";

(function(){

  angular.module('crossroads').directive("kidsClubAdultApplication", KidsClubAdultApplication);

  KidsClubAdultApplication.$inject = ['$log', '$rootScope'];

  function KidsClubAdultApplication($log, $rootScope){

    return {
      restrict: "EA",
      templateUrl : "kc_adult_application/kidsClubAdultApplication.template.html",
      controller: "KidsClubAdultApplicationController as kcAdultApplication",
      scope: {
        volunteer: "=volunteer",
        contactId: '=contactId',
        opportunityId: '=opportunityId',
        responseId: '=responseId',
        showSuccess: '=showSuccess'
      },
      bindToController: true
    }; 
  }

})();
