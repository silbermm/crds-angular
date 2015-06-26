"use strict";
(function() {
    angular.module("crossroads.core").run( AppRun );

    AppRun.$inject = ["Session", "$rootScope", "MESSAGES", "$http", "$log", "$state", "$timeout", "$location"];

    function AppRun(Session, $rootScope, MESSAGES, $http, $log, $state, $timeout, $location) {
        $rootScope.MESSAGES = MESSAGES;

        function clearAndRedirect(event, toState,toParams) {
            // TODO Added to debug/research US1403 - should remove after issue is resolved
            console.log("US1403: clearAndRedirect to " + toState.name + " in app.run");
            console.log($location.search());

            Session.clear();
            $rootScope.userid = null;
            $rootScope.username = null;
            Session.addRedirectRoute(toState.name, toParams.link, toParams);
            event.preventDefault();
            $state.go("login");
        }

        $rootScope.$on("$stateChangeStart", function(event, toState, toParams, fromState, fromParams) {
           if (Session.isActive()) {
              // TODO Added to debug/research US1403 - should remove after issue is resolved
              console.log("US1403: stateChangeStart event handler (" + fromState.name + "->" + toState.name + "), session active, in app.run");
              $http({
                method: "GET",
                url :__API_ENDPOINT__ + "api/authenticated",
                withCredentials: true,
                headers: {
                  'Authorization': crds_utilities.getCookie('sessionId')
                }}).success(function (user) {
                    // TODO Added to debug/research US1403 - should remove after issue is resolved
                    console.log("US1403: stateChangeStart event handler, successful call to api/authenticated in app.run");
                    $rootScope.userid = user.userId;
                    $rootScope.username = user.username;
                }).error(function (e) {
                    // TODO Added to debug/research US1403 - should remove after issue is resolved
                    console.log("US1403: stateChangeStart event handler, failed call to api/authenticated in app.run");
                    clearAndRedirect(event, toState, toParams);
                });
            } else if (toState.data !== undefined && toState.data.isProtected) {
                // TODO Added to debug/research US1403 - should remove after issue is resolved
                console.log("US1403: stateChangeStart event handler (" + fromState.name + "->" + toState.name + "), no session w/protected data, in app.run");
                clearAndRedirect(event, toState, toParams);
            }
        });
    };
})();
