(function () {
    angular.module("crdsProfile").directive("crdsPassword", ['$log',CrdsPassword]);

    function CrdsPassword($log) {
        return {
            restrict: 'E',
            replace: true,
            scope: true,
            contoller: 'crdsProfileCtrl as profile',
            templateUrl: 'app/modules/profile/templates/profile_password.html',
            link: (function (scope, el, attr) {
                scope.showPassword = false;

            })
        }
    }
})()