(function(){
    angular.module('crdsProfile').factory('Lookup', ["$resource", "Session", LookupService]);

    function LookupService($resource, Session) {
        return {
            Genders: $resource("api/lookup/311"),
            MaritalStatus: $resource("api/lookup/339"),
            ServiceProviders: $resource("api/lookup/453"),
            States: $resource("api/lookup/452"),
            Countries: $resource("api/lookup/442"),
            CrossroadsLocations: $resource("api/lookup/466")
        }
    }
})()