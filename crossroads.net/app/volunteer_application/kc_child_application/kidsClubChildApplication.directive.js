"use strict()";

(function(){

  module.exports = KidsClubChildApplication;

  KidsClubChildApplication.$inject = ['$log'];

  function KidsClubChildApplication($log){

    return {
      restrict: "EA",
      templateUrl : "kc_child_application/kidsClubChildApplication.template.html",
      link: link
    };

    function link(scope, el, attr) {
      $log.debug('Kids-Club-Child-Application directive');
    }
  }

})();
