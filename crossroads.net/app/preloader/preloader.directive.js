'use strict()';
(function(){
  angular.module('crossroads').directive("preloader", preloader);

  preloader.$inject = [];

  function preloader(){
    return {
      restrict: 'EA',
      templateUrl: 'preloader/preloader.html'
    }
  }
})();
