require('./session_service');
(function () {
  'use strict';
    angular.module('crossroads.core').factory('AuthService', ['$http', 'Session',
    '$rootScope', 'AUTH_EVENTS', function ($http, Session, $rootScope, AUTH_EVENTS) {
        var authService = {};

        authService.login = function (credentials) {
            return $http
                .post(__API_ENDPOINT__ + 'api/login', credentials)
                .then(function (res) {
                    console.log(res.data);
                    Session.create(res.data.userToken, res.data.userId, res.data.username);
                    // The username from the credentials is really the email address
                    // In a future story, the contact email address will always be in sync with the user email address.
                    $rootScope.email = credentials.username;
                    $rootScope.username = res.data.username;
                    $rootScope.roles = res.data.roles;
                    $rootScope.userid = res.data.userId;
                    return res.data.username;
                });
        };

        authService.logout = function () {
            // TODO Added to debug/research US1403 - should remove after issue is resolved
            console.log('US1403: logging out user in auth_service');
            $rootScope.email = null;
            $rootScope.username = null;
            $rootScope.userid = null;
            $rootScope.roles = null;
            Session.clear();
            $rootScope.$broadcast(AUTH_EVENTS.logoutSuccess);
        };

        //We are pretty sure they are copied from an example of how WE SHOULD be doing this,
        //instead we are using the above rootScope
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
    }]);
})();
