(function () {
    angular.module("password_field").directive("passwordField", ['$log', 'Profile', PasswordField]);

    function PasswordField($log, Profile) {
        return {
            restrict: 'EA',
            replace: true,
            scope: {
                passwd: "=passwd"
            },
            templateUrl: 'app/modules/password_field/password_field.html',
            link: (function (scope, el, attr, ctrl) {
                //scope.showPassword = false;
                scope.AAAinputType = 'password';
                scope.AAApwprocessing = "SHOW";
                scope.AAApwprocess = function () {
                    $log.debug("pwprocess function launched");
                    if (scope.AAApwprocessing == "SHOW") {
                        scope.AAApwprocessing = "HIDE";
                        scope.AAAinputType = 'text';
                    }
                    else {
                        scope.AAApwprocessing = "SHOW";
                        scope.AAAinputType = 'password';
                    }
                    $log.debug(scope.AAApwprocessing);
                }
            })
        }
    }
})()