'use strict';
(function(){
    angular.module('crossroads').factory('AuthService', ['$http', 'Session', function ($http, Session) {

        var authService = {};

        authService.login = function (credentials) {
            console.log("credentials: " + credentials);
            return $http
                .post('api/login', credentials)
                .then(function (res) {
                    Session.create(res.data.userToken, res.data.userId);
                    return {"id": res.data.userId};
                });
        };

        authService.isAuthenticated = function () {
            if (Session.authenticated()) {
                var isAuth = $http.get("api/authenticated").then(function (res) {
                    Session.create(res.data.userToken, res.data.userId);
                    return true;
                }, function (res) {
                    Session.clear();
                    return false;
                });
                return authObject;
            } else {
                return false;
            }
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