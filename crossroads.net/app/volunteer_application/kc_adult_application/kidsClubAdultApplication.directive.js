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
 
      scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1,
        showWeeks: 'false'
      };
      scope.datePickers = { childDob1 : false, childDob2: false };
      scope.format = 'MM/dd/yyyy';
      scope.open = open;
      scope.religion = { 1: "exploring", 2: "notsure", 3: "christ" }
      scope.religionSelected = religionSelected;
      scope.save = save;
      

      //////////////////////////////////////
      
      function open(field, $event) {
        $event.preventDefault();
        $event.stopPropagation();
        scope.datePickers[field] = true;
        console.log(scope.datePickers);
      }

      function save(){
        $log.debug("saving");
        if(scope.adult.$invalid){
          $log.error("please fill out all required fields correctly"); 
          return false;
        } 
        $log.debug("Thank you for filling out the form");
        return true;
      }

      function religionSelected(){
        if (scope.volunteer.person.exploring || scope.volunteer.person.unsure || scope.volunteer.person.christ)
          return true;
        return false;
      }

    }
  }

})();
