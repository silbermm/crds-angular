'use strict';
(function(){
    angular.module('crossroads').factory('AuthService', ['$http', 'Session', function ($http, Session) {
        var authService = {};

        authService.login = function (credentials) {
            return $http
                .post('api/login', credentials)
                .then(function (res) {
                    Session.create(res.data.id, res.data.username);
                    return res.data.username;
                });
        };

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