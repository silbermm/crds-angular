(function(){
    angular.module('crossroads').directive('registerForm', ["$log", "AUTH_EVENTS", RegisterForm]);

    function RegisterForm($log, AUTH_EVENTS){
        return {
            restrict: 'EA',
            templateUrl: "app/crossroads.net/register/register_form.html",
            controller: "RegisterCtrl",
            link: function (scope) {
                $log.debug("in the registerform directive");
                var showForm = function () {
                    $log.debug('not logged in');
                    scope.visible = true;
                };

                scope.visible = false;
               
            }
        };
    }
})()