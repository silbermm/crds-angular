'use strict';
(function () {
    angular.module('crossroads', ['crdsProfile', 'ui.router'])
    .run(function ($rootScope, AUTH_EVENTS, AuthService) {
        $rootScope.$on('$stateChangeStart', function(event, next) {        
            var requireLogin = next.data.require_login;
            if (requireLogin) {
                if (AuthService.isAuthenticated()) {
                    // user is not allowed
                    $rootScope.$broadcast(AUTH_EVENTS.notAuthorized);
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
            notAuthorized: 'auth-not-authorized'
     })
    .controller('appCtrl', ['$scope', 'AuthService', function ($scope, AuthService) {
        $scope.main = "appCtrl";

        $scope.currentUser = null;
        $scope.isAuthorized = AuthService.isAuthorized;
        $scope.isLoginPage = false;

        console.log('isLoginPage: ' + $scope.isLoginPage);

        $scope.setCurrentUser = function (user) {
            $scope.currentUser = user;
        };
    }]);
})()