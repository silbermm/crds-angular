(function () {
    angular.module('crossroads').
        factory('Group', ['$resource','$log','$stateParams', GroupService]);
        
    function GroupService($resource, $log, $stateParams) {
        $log.debug("Inside Group factory");
        return $resource( __API_ENDPOINT__ +  'api/group/'+$stateParams.groupId+'/user');
    }

})()
