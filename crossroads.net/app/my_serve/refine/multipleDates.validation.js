(function () {
  
  angular.module('crossroads').directive('multipleDates', MultipleDates);

  function MultipleDates(){
    return {
      require: 'ngModel', 
      link: linkFunc 
    };

    function linkFunc(scope, element, attrs, ngModel) {
      // from date cannot be less than today
      //
      ngModel.$validators.fromDate = function (value) {
        console.log(attrs);
        return true;
      };

      // to date has to be after from date
      ngModel.$validators.toDate = function(value){
        console.log(attrs);
        return false;
      };
    }

  }

})();

