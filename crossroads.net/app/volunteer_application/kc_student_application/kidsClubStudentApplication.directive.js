"use strict()";

(function(){

  module.exports = KidsClubCStudentApplication;

  KidsClubStudentApplication.$inject = ['$log'];

  function KidsClubStudentApplication($log){

    return {
      restrict: "EA",
      templateUrl : "kc_student_application/kidsClubStudentApplication.template.html",
      link: link
    };

    function link(scope, el, attr) {
      $log.debug('Kids-Club-Student-Application directive');
    }
  }

})();
