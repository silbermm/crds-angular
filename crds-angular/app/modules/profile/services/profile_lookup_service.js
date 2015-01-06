(function(){
    angular.module('crdsProfile').factory('Lookup', ["$resource", "Session", LookupService]);

    function LookupService($resource, Session) {
        return {
            Fetch: $resource("api/lookup/"),
            Genders: $resource("api/lookup/?table=genders"),
            MaritalStatus: $resource("api/lookup/339"),
            ServiceProviders: $resource("api/lookup/453"),
            States: $resource("api/lookup/452"),
            Countries: $resource("api/lookup/442"),
            CrossroadsLocations: $resource("api/lookup/466"),
            Skills: $resource("api/skill")
        }
    }
})()