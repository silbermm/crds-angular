(function(){
    angular.module('crdsProfile').factory('Lookup', ["$resource", "Session", LookupService]);

    function LookupService($resource, Session) {
        return $resource("api/lookup/");
    }
})()