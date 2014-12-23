'use strict';
(function(){
    angular.module('crossroads').factory('AuthService', ['$http', 'Session', '$rootScope', function ($http, Session, $rootScope) {

        var authService = {};

        authService.login = function (credentials) {
            return $http
                .post('api/login', credentials)
                .then(function (res) {
                    Session.create(res.data.userToken, res.data.userId, res.data.username);
                    return {"id": res.data.userId};
                });
        };

        authService.logout = function () {
            Session.clear();
            $rootScope.isAuthenticated = false;
        };

        authService.isAuthenticated = function () {
            if (Session.exists("sessionId")) {
                var isAuth = $http.get("api/authenticated").then(function (res) {
                    console.log ("auth??");
                    console.log(res.data);
                    Session.create(res.data.userToken, res.data.userId, res.data.username);
                    return true;
                }, function (res) {
                    $log.debug("authService.isAuthenticated() :: not authenticated!");
                    Session.clear();
                    return false;
                });
                return isAuth;
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