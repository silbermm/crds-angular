(function(){
    angular.module('crossroads').directive('loginDialog', ["$log", "AUTH_EVENTS", LoginDialog]);

    function LoginDialog($log, AUTH_EVENTS){
        return {
            restrict: 'EA',
            templateUrl: "app/crossroads.net/login/login_dialog.html",
            controller: "LoginCtrl",
            link: function (scope) {
                $log.debug("in the logindialog directive");
                var showDialog = function () {
                    $log.debug('not logged in');
                    scope.visible = true;
                };

                scope.visible = false;
                scope.$on(AUTH_EVENTS.notAuthenticated, showDialog);
                scope.$on(AUTH_EVENTS.sessionTimeout, showDialog);
            }
        };
    }
})()