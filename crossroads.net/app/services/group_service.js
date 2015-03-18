(function () {
    angular.module('crossroads').
        factory('Group', ['$resource','$log', GroupService]);
        
    function GroupService($resource, $log) {
        $log.debug("Inside Group factory");
        return $resource( __API_ENDPOINT__ +  'api/group/:groupId/user', {
          groupId : '@groupId'
        });
    }

})()
