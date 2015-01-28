(function () {
    angular.module("crdsProfile").directive("crdsPassword", ['$log', 'Profile', CrdsPassword]);

    function CrdsPassword($log, Profile) {
        return {
            restrict: 'E',
            transclude: true,
            //replace: true,
            //scope: true,
            templateUrl: 'app/modules/profile/directives/profile_password.html',
            link: (function (scope, el, attr, ctrl) {
                scope.showPassword = false;

            })
        }
    }
})()