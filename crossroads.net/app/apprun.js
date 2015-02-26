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
                $http.get(__API_ENDPOINT__ + "api/authenticated").success(function (user) {
                    $rootScope.userid = user.userId;
                    $rootScope.username = user.username;                    
                }).error(function (e) {
                    clearAndRedirect(event, toState, toParams);
                });
            } else if (toState.data !== undefined && toState.data.isProtected) {              
                clearAndRedirect(event, toState, toParams);
            } else {
                //There is no session AND the user is not attempting to go to a protected route
                //so there is nothing to do
            }
 
        });
    };
    angular.module("crossroads").run(["Session", "$rootScope", "MESSAGES", "$http", "$log", "$state", "$timeout", AppRun]);
})();
