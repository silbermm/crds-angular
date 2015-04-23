'use strict()';
(function(){
  angular.directive("preLoader", preloader);

  preloader.$inject = [];

  function preloader(){
    return {
      restrict: 'EA',
      templateUrl: 'preloader/preloader.html'
    }
  }
})();
