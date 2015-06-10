"use strict()";

(function(){

  angular.module('crossroads').directive("kidsClubAdultApplication", KidsClubAppliation);

  KidsClubAdultApplication.$inject = ['$log', '$rootScope'];

  function KidsClubAdultApplication($log, $rootScope){

    return {
      restrict: "EA",
      templateUrl : "kc_adult_application/kidsClubAdultApplication.template.html",
      scope: {
        volunteer: "=volunteer" 
      },
      link: link
    };

    function link(scope, el, attr) {
      $log.debug('KidsClubAdultApplication directive');

      /**
       * Setup all bindable members
       */
      scope.availabilitySelected = availabilitySelected;
      scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1,
        showWeeks: 'false'
      };
      scope.datePickers = { childDob1 : false, childDob2: false, signatureDate: false };
      scope.format = 'MM/dd/yyyy';
      scope.gradeLevelSelected = gradeLevelSelected;
      scope.locationSelected = locationSelected;
      scope.open = open;
      scope.religionSelected = religionSelected;
      scope.save = save;
      

      //////////////////////////////////////
      
      /**
       * Checks if one of the availabilities has been selected and returns
       * true if it has, false otherwise
       */
      function availabilitySelected(){
        if (scope.volunteer.availabilityWeek || scope.volunteer.availabilityWeekend)
          return true;
        return false;
      }

      /**
       * Checks if one of the grade levels has been selected and 
       * returns true if has, false otherwise
       */
      function gradeLevelSelected(){
        if (scope.volunteer.birthToTwo ||
            scope.volunteer.threeToPreK ||
            scope.volunteer.kToFifth)
          return true;
        return false;
      }

      /**
       * Checks if one of the availability locations has been selected and returns
       * true if it has, false otherwise
       */
      function locationSelected(){
        if (scope.volunteer.availabilityOakley 
            || scope.volunteer.availabilityFlorence
            || scope.volunteer.availabilityWestside
            || scope.volunteer.availabilityMason
            || scope.volunteer.availabilityClifton)
          return true;
        return false;
      }

      /**
       * Open the date picker for the passed in field
       */
      function open(field, $event) {
        if($event !== null){
          $event.preventDefault();
          $event.stopPropagation();
        }
        scope.datePickers[field] = true;
      }

      /**
       * Attempt to save the form response 
       */
      function save(){
        $log.debug("saving");
        if(scope.adult.$invalid){
          $log.error("please fill out all required fields correctly"); 
          $rootScope.$emit('notify',$rootScope.MESSAGES.generalError);
          return false;
        } 
        $log.debug("Thank you for filling out the form");
        return true;
      }

      /**
       * Checks if one of the a religion options has been selected and returns
       * true if it has, false otherwise
       */
      function religionSelected(){
        if (scope.volunteer.exploring || scope.volunteer.unsure || scope.volunteer.christ)
          return true;
        return false;
      }

    }
  }

})();
