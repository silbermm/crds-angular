'use strict';
(function () {
    angular.module('crossroads', ['crdsProfile', 'ui.router', 'ngCookies'])
    .run(function ($rootScope, AUTH_EVENTS, AuthService) {
        $rootScope.$on('$stateChangeStart', function(event, next) {        
            var requireLogin = next.data.require_login;
            if (requireLogin) {
                if (AuthService.isAuthenticated()) {
                    // user is not allowed
                    $rootScope.$broadcast(AUTH_EVENTS.isAuthenticated);
                } else {
                    // user is not logged in
                    event.preventDefault();
                    $rootScope.$broadcast(AUTH_EVENTS.notAuthenticated);
                }
            }
        });
    })
    .constant('AUTH_EVENTS', {
            loginSuccess: 'auth-login-success',
            loginFailed: 'auth-login-failed',
            logoutSuccess: 'auth-logout-success',
            sessionTimeout: 'auth-session-timeout',
            notAuthenticated: 'auth-not-authenticated',
            isAuthenticated : 'auth-is-authenticated',
            notAuthorized: 'auth-not-authorized'
     })
    .controller('appCtrl', ['$scope', '$rootScope', 'AuthService', function ($scope, $rootScope, AuthService) {
        $scope.main = "appCtrl";

        $rootScope.currentUser = null;
        $scope.isAuthorized = AuthService.isAuthorized;
        $scope.isLoginPage = false;
        $rootScope.setCurrentUser = function (user) {
            $scope.currentUser = user;
        };
    }]);
})()