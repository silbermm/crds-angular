(function(){
    angular.module('crossroads').directive('loginForm', ["$log", "AUTH_EVENTS", LoginForm]);

    function LoginForm($log, AUTH_EVENTS){
        return {
            restrict: 'EA',
            templateUrl: "app/crossroads.net/login/login_form.html",
            controller: "LoginCtrl",
            link: function (scope) {
                var showForm = function () {
                    $log.debug('not logged in');
                    scope.visible = true;
                };

                scope.visible = false;
               
            }
        };
    }
})()