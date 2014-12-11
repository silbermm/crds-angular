'use strict';
(function () {
    angular.module('crossroads').controller('LoginCtrl', ['$rootScope', 'AUTH_EVENTS', 'AuthService', LoginController]);

    function LoginController($rootScope, AUTH_EVENTS, AuthService) {
        this.main = "LoginCtrl";
       
        this.credentials = { username: '', password: '' };
        this.isLoginPage = true;
        
        this.login = function (credentials) {
            AuthService.login(credentials).then(function (user) {
                $rootScope.$broadcast(AUTH_EVENTS.loginSuccess);
                $scope.setCurrentUser(user);
            }, function () {
                $rootScope.$broadcast(AUTH_EVENTS.loginFailed);
            });
        };
    }
})()