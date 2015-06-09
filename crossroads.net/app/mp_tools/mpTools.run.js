"use strict";
(function() {
    angular.module("crossroads.mptools").run( AppRun );

    AppRun.$inject = ["$location", 'MPTools'];
    //AppRun.$inject = ["Session", "$rootScope", "MESSAGES", "$http", "$log", "$state", "$timeout", "$location"];

    // function AppRun(Session, $rootScope, MESSAGES, $http, $log, $state, $timeout, $location) {
    function AppRun($location, MPTools) {

        if($location.search()['ug'] !== undefined){
            MPTools.setParams($location);
        }
    };
})();
