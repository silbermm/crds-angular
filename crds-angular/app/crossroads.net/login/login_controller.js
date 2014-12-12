'use strict';
(function () {
    angular.module('crossroads').controller('LoginCtrl', ['$scope', '$rootScope', 'AUTH_EVENTS', 'AuthService', '$state', '$log',  LoginController]);

    function LoginController($scope, $rootScope, AUTH_EVENTS, AuthService, $state, $log) {
        $scope.main = "LoginCtrl";
       
        $scope.credentials = { username: '', password: '' };
        $scope.isLoginPage = true;
        
        $scope.login = function (credentials) {
            AuthService.login(credentials).then(function () {
                $log.debug("got a 200 from the server "); 
                $state.go('profile');
            }, function () {
                $log.debug("Bad password");
                //$rootScope.$broadcast(AUTH_EVENTS.loginFailed);
            });
        };
    }
})()