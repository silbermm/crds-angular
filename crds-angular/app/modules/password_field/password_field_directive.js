(function () {
    angular.module("password_field").directive("passwordField", ['$log', 'Profile', PasswordField]);

    function PasswordField($log, Profile) {
        return {
            restrict: 'EA',
            transclude: true,
            replace: true,
            scope: true,
            templateUrl: 'app/modules/password_field/password_field.html',
            link: (function (scope, el, attr, ctrl) {
                //scope.showPassword = false;

            })
        }
    }
})()