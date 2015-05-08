"use strict";
(function() {
    var getCookie = require('./utilities/cookies');
    angular.module("crossroads").run( AppRun );

    AppRun.$inject = ["Session", "$rootScope", "MESSAGES", "$http", "$log", "$state", "$timeout", "$location", 'MPTools'];

    function AppRun(Session, $rootScope, MESSAGES, $http, $log, $state, $timeout, $location, MPTools) {
        $rootScope.MESSAGES = MESSAGES;

        function clearAndRedirect(event, toState,toParams) {
            console.log($location.search()); 
            if($location.search()['ug'] !== undefined){
              MPTools.setParams($location); 
            }
            Session.clear();
            $rootScope.userid = null;
            $rootScope.username = null;
            Session.addRedirectRoute(toState.name, toParams.link);
            event.preventDefault();
            $state.go("login");
        }
        
        $rootScope.$on("$stateChangeStart", function(event, toState, toParams, fromState, fromParams) {      
           if (Session.isActive()) {
              $http({
                method: "GET",
                url :__API_ENDPOINT__ + "api/authenticated", 
                withCredentials: true, 
                headers: {
                  'Authorization': getCookie('sessionId')
                }}).success(function (user) {
                    $rootScope.userid = user.userId;
                    $rootScope.username = user.username;                    
                }).error(function (e) {
                    clearAndRedirect(event, toState, toParams);
                });
            } else if (toState.data !== undefined && toState.data.isProtected) {    
                clearAndRedirect(event, toState, toParams);
            } 
        });
    };
})();
