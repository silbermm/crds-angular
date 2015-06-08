"use strict()";

(function(){

  module.exports = KidsClubAdultApplication;

  KidsClubAdultApplication.$inject = ['$log'];

  function KidsClubAdultApplication($log){

    return {
      restrict: "EA",
      templateUrl : "kc_adult_application/kidsClubAdultApplication.template.html",
      link: link
    };

    function link(scope, el, attr) {
      $log.debug('KidsClubAdultApplication directive');
    }
  }

})();
