'use strict()';
(function() {

    module.exports = function() {

      var REQUIRED_PATTERNS = [
        /^[1-9]\d*$/
      ];

      return {
        restrict: 'A',
        require: 'ngModel',
        link: function($scope, element, attrs, ngModel) {
          ngModel.$validators.naturalNumber = function(value) {
            var status = true;
            angular.forEach(REQUIRED_PATTERNS, function(pattern) {
              status = status && pattern.test(value);
            });
            return status;
          };
        }
      }
    };
})()