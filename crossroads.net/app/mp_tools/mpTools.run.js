"use strict";
(function() {
    angular.module("crossroads.mptools").run( AppRun );

    AppRun.$inject = ["$location", 'MPTools'];

    function AppRun($location, MPTools) {

        if($location.search()['ug'] !== undefined){
            MPTools.setParams($location);
        }
    };
})();
