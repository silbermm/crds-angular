'use strict';

angular.module('crossroads')

.directive('authForm', function() {
  return {
    restrict: 'EA',
    templateUrl: 'crossroads.net/templates/login.html',
    controller: 'LoginCtrl',
    priority: 0
  };
});
