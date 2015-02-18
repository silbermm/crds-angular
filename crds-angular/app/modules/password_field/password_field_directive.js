(function () {
    angular.module("password_field",[]).directive("passwordField", ['$log', 'Profile', PasswordField]);

    function PasswordField($log, Profile) {
        return {
            restrict: 'EA',
            replace: true,
            scope: {
                passwd: "=passwd",
                submitted: "=",
                prefix: "=",
                required: "@required"
            },
            templateUrl: 'app/modules/password_field/password_field.html',
            link: (function (scope, el, attr, ctrl) {
                //scope.showPassword = false;
                scope.inputType = 'password';
                scope.pwprocessing = "SHOW";
                scope.pwprocess = function () {                    
                    if (scope.pwprocessing == "SHOW") {
                        scope.pwprocessing = "HIDE";
                        scope.inputType = 'text';
                    }
                    else {
                        scope.pwprocessing = "SHOW";
                        scope.inputType = 'password';
                    }
                    $log.debug(scope.pwprocessing);
                }
            })
        }
    }
})()