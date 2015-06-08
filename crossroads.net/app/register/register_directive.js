require('./register_form.html');
require('./register_controller');
(function(){
    angular.module('crossroads.core').directive('registerForm', ["$log", "AUTH_EVENTS", RegisterForm]);

    function RegisterForm($log, AUTH_EVENTS){
        return {
            restrict: 'EA',
            templateUrl: "register/register_form.html",
            controller: "RegisterCtrl",
            link: function (scope, element, attrs) {
                $log.debug("in the registerform directive");
                if(attrs.prefix){
                  scope.passwordPrefix = attrs.prefix;
                }
                var showForm = function () {
                    $log.debug('not logged in');
                    scope.visible = true;
                };

                scope.visible = false;

            }
        };
    }
})()
