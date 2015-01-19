(function(){
    angular.module('crossroads').directive('loginForm', ["$log", "AUTH_EVENTS", LoginDialog]);

    function LoginDialog($log, AUTH_EVENTS){
        return {
            restrict: 'EA',
            templateUrl: "app/crossroads.net/login/login_form.html",
            controller: "LoginCtrl",
            link: function (scope) {
                $log.debug("in the loginForm directive");
                var showDialog = function () {
                    $log.debug('not logged in');
                    scope.visible = true;
                };

                scope.visible = false;
               
            }
        };
    }
})()