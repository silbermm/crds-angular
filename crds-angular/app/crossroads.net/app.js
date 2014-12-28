'use strict';
(function () {
    angular.module('crossroads', ['crdsProfile', 'ui.router', 'ngCookies'])
    .run(['Session', '$rootScope',function(Session, $rootScope){
        $rootScope.username = Session.exists('username');
    }])
    .constant('AUTH_EVENTS', {
            loginSuccess: 'auth-login-success',
            loginFailed: 'auth-login-failed',
            logoutSuccess: 'auth-logout-success',
            sessionTimeout: 'auth-session-timeout',
            notAuthenticated: 'auth-not-authenticated',
            isAuthenticated : 'auth-is-authenticated',
            notAuthorized: 'auth-not-authorized'
     })
    .controller('appCtrl', ['$scope', '$rootScope', function ($scope, $rootScope) {
        
    }]);
})()