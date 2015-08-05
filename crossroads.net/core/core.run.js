(function() {
    'use strict';
    angular.module('crossroads.core').run( AppRun );

    AppRun.$inject = ['Session',
        '$rootScope',
        'MESSAGES',
        '$http',
        '$log',
        '$state',
        '$timeout',
        '$location',
        '$cookies'];

    function AppRun(Session, $rootScope, MESSAGES, $http, $log, $state, $timeout, $location, $cookies) {
        $rootScope.MESSAGES = MESSAGES;

        function clearAndRedirect(event, toState,toParams) {
            console.log($location.search());

            Session.clear();
            $rootScope.userid = null;
            $rootScope.username = null;
            Session.addRedirectRoute(toState.name, toParams);
            event.preventDefault();
            $state.go('login');
        }

        $rootScope.$on('$stateChangeStart', function(event, toState, toParams, fromState, fromParams) {
            if (toState.name === 'logout') {
                if (fromState.data === undefined || !fromState.data.isProtected) {
                    Session.addRedirectRoute(fromState.name, fromParams);
                }
                else {
                    Session.addRedirectRoute('home', '');
                }
                return;
           }
	    
           if (toState.data !== undefined && toState.data.preventRouteAuthentication) {
             return;
           }
           if (Session.isActive()) {
              $http({
                method: 'GET',
                url :__API_ENDPOINT__ + 'api/authenticated',
                withCredentials: true,
                headers: {
                  'Authorization': $cookies.get('sessionId')
                }}).success(function (user) {
                    $rootScope.userid = user.userId;
                    $rootScope.username = user.username;
                    $rootScope.roles = user.roles;
                }).error(function (e) {
                    clearAndRedirect(event, toState, toParams);
                });
            } else if (toState.data !== undefined && toState.data.isProtected) {
                clearAndRedirect(event, toState, toParams);
            } else {
                $rootScope.userid = null;
                $rootScope.username = null;
            }
        //}
        });
    }
})();
