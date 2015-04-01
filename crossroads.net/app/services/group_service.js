(function () {
  angular.module('crossroads').
  factory('Group', ['$resource','$log', GroupService]);

  function GroupService($resource, $log) {
    $log.debug("Inside Group factory");
    return{
      Participant: $resource( __API_ENDPOINT__ +  'api/group/:groupId/participants', {groupId : '@groupId'},
        {'save':   {method:'POST', isArray:true}}),
      Detail: $resource( __API_ENDPOINT__ +  'api/group/:groupId', {groupId : '@groupId'})
    }
  }

})()
