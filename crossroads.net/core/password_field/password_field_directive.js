require('./password_field.html');﻿

(function () {
    angular.module("crossroads.core").directive("passwordField", ['$log', PasswordField]);

    function PasswordField($log) {
        return {
            restrict: 'EA',
            replace: true,
            scope: {
                passwd: "=passwd",
                submitted: "=",
                prefix: "=prefix",
                required: "@required"
            },
            templateUrl: 'password_field/password_field.html',
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