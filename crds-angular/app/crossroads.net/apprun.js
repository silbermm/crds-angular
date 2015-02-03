"use strict";
(function() {

    function AppRun(Session, $rootScope, MESSAGES, $http, $log, $state, $timeout) {

        $rootScope.MESSAGES = MESSAGES;

        function clearAndRedirect(event, toState,toParams) {
            Session.clear();
            $rootScope.userid = null;
            $rootScope.username = null;
            Session.addRedirectRoute(toState.name, toParams);
            event.preventDefault();
            $state.go("login");
        }
        
        $rootScope.$on("$stateChangeStart", function(event, toState, toParams, fromState, fromParams) {           
            if (Session.isActive()) {
                $log.debug("Session is active, check if authenticated");
                $http.get("api/authenticated").success(function (user) {
                    $log.debug("No need to clear session");
                    $rootScope.userid = user.userId;
                    $rootScope.username = user.username;                    
                }).error(function (e) {
                    clearAndRedirect(event, toState, toParams);
                });
            } else if (toState.data !== undefined && toState.data.isProtected) {
                $log.debug("session is not active, but we are trying to access a protected state");
                clearAndRedirect(event, toState, toParams);
            }
            
            
        });

       


    };

    angular.module("crossroads").run(["Session", "$rootScope", "MESSAGES", "$http", "$log", "$state", "$timeout", AppRun]);
})();