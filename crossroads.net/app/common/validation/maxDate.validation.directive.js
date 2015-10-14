(function() {
  'use strict';
  module.exports = function() {

    return {
      restrict: 'A',
      require: 'ngModel',
      link: function(scope, element, attrs, ngModel) {
        ngModel.$validators.maxDate = function(value) {
          return true;
        };
      }

    };
  };
})();
