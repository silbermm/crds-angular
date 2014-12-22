'use strict';
(function () {
    angular.module('crossroads', ['crdsProfile', 'ui.router', 'ngCookies'])
    .run(function ($rootScope, AUTH_EVENTS, AuthService) {
        $rootScope.$on('$stateChangeStart', function(event, next) {        
            var requireLogin = next.data.require_login;
            if (requireLogin) {
                if (AuthService.isAuthenticated()) {
                    console.log("user is authenticated");
                    $rootScope.$broadcast(AUTH_EVENTS.isAuthenticated);
                } else {
                    // user is not logged in
                    console.log("user is not authenticated");
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
        console.log("in here now");
        $rootScope.isAuthenticated = AuthService.isAuthenticated();
        console.log("in here now 2");
        $rootScope.isAuthorized = AuthService.isAuthorized();
        $scope.isLoginPage = false;
        $rootScope.setCurrentUser = function (user) {
            $scope.currentUser = user;
        };
    }]);
})()