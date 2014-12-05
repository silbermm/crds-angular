angular.module('crossroads').directive('loginDialog', function (AUTH_EVENTS) {
    return {
        restrict: 'A',
        template: '<div ng-if="visible" ng-include="\'/app/crossroads.net/login/login-dialog.html\'">',
        link: function (scope) {
            var showDialog = function () {
                console.log('not logged in');
                scope.visible = true;
            };

            scope.visible = false;
            scope.$on(AUTH_EVENTS.notAuthenticated, showDialog);
            scope.$on(AUTH_EVENTS.sessionTimeout, showDialog);
        }
    };
});