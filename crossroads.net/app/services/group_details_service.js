(function () {
    angular.module('crossroads').
        factory('Group', ['$resource','$log', GroupDetailsService]);

    function GroupDetailsService($resource, $log) {
        $log.debug("Inside Group Details Svc");
        return $resource( __API_ENDPOINT__ +  'api/group/:groupId', {
          groupId : '@groupId'
        });
    }

})()
