(function(){
    angular.module('crdsProfile').factory('Lookup', ["$resource", "Session", LookupService]);

    function LookupService($resource, Session) {
        return $resource(__API_ENDPOINT__ + "api/lookup/", null, null, {
          withCredentials: true,
          headers: {
            'Authorization': Session.exists('sessionId')
          }
        });
    }
})()
