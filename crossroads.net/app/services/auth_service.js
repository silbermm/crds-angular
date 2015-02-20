'use strict';
require('./session_service');
(function () {
    angular.module('crossroads').factory('AuthService', ['$http', 'Session', '$rootScope', function ($http, Session, $rootScope) {
        var authService = {};

        authService.login = function (credentials) {
            return $http
                .post('api/login', credentials)
                .then(function (res) {
                    console.log(res.data);
                    Session.create(res.data.userToken, res.data.userId, res.data.username);
                    $rootScope.username = res.data.username;
                    return res.data.username;
                });
        };

        authService.logout = function () {
            $rootScope.username = null;
            Session.clear();
        }

        authService.isAuthenticated = function () {
            return !!Session.userId;
        };

        authService.isAuthorized = function (authorizedRoles) {
            if (!angular.isArray(authorizedRoles)) {
                authorizedRoles = [authorizedRoles];
            }
            return (authService.isAuthenticated() &&
                authorizedRoles.indexOf(Session.userRole) !== -1);
        };

        return authService;
    }])
})()