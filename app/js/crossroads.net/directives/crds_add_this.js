'use strict';

angular.module('crossroads')

.directive('addThis', function() {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: 'crossroads.net/templates/addthis.html'
    };
});

