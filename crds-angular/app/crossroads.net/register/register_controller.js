'use strict';
(function () {
    angular.module('crossroads').controller('RegisterCtrl', ['$scope', '$rootScope', 'AUTH_EVENTS', 'MESSAGES', 'AuthService', '$cookieStore', '$state', '$log', "Session", RegisterController]);

    function RegisterController($scope, $rootScope, AUTH_EVENTS, MESSAGES, AuthService, $cookieStore, $state, $log, Session) {
        $scope.showRegisterButton = true;
        $scope.registerShow = false;

        $scope.toggleDesktopRegister = function () {
            $scope.registerShow = !$scope.registerShow;
            if ($scope.loginShow)
                $scope.loginShow = !$scope.loginShow;
        }
    }
})()