'use strict';
(function(){
    angular.module('crossroads').factory('AuthService', ['$http', '$scope', 'Session', function ($http, $scope, Session) {
        var authService = {};

        authService.login = function (credentials) {
            console.log("credentials: " + credentials);
            return $http
                .post('api/login', credentials)
                .then(function (res) {
                    Session.create(res.data.id, res.data.username);
                    return {"id": res.data.id, "username": res.data.username};
                });
        };

        authService.isAuthenticated = function () {
            return !!Session.authenticated();
        };

        authService.isAuthorized = function (authorizedRoles) {
            if (!angular.isArray(authorizedRoles)) {
                authorizedRoles = [authorizedRoles];
            }
            return (authService.isAuthenticated() && authorizedRoles.indexOf(Session.getUserRole()) !== -1);
        };

        return authService;
    }])
})()