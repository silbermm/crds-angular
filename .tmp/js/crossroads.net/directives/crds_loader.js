'use strict';

angular.module('crossroads').directive('crdsProgressBar', function() {
    return {
        restrict: 'E',
        templateUrl: 'crossroads.net/templates/crdsProgressbar.html',
        scope: {
            percentage: '@'
        }
    };
});
