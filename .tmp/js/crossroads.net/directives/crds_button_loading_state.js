'use strict';

angular.module('crossroads')

.directive('crdsLoadingText', function() {
  return {
    restrict: 'A',
    scope: {
      'crdsLoadingText': '@',
      'crdsLoading': '='
    },
    link: function($scope, $element, $attrs) {
      var baseText = $element.html(),
          baseDisabled = $attrs.disabled;

      $scope.$watch('crdsLoading', function(value) {
        var text;

        if (value) {
          $attrs.$set('disabled', true);
          text = $scope.$parent.$eval($scope.crdsLoadingText);

          if (!text) {
            text = $scope.crdsLoadingText;
          }

          $element.html(text);

        } else {
          $element.html(baseText);

          $attrs.$set('disabled', baseDisabled);
        }
      });

      $scope.$parent.$watch($attrs.ngDisabled, function(value) {
        baseDisabled = value;
      });
    }
  };
});
