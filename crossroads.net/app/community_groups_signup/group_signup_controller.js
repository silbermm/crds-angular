'use strict';
(function () {
    module.exports = function GroupSignupController($scope, $rootScope, Profile, $log, $stateParams) {
        $log.debug("Inside GroupSignupController");
        Profile.Personal.get(function(response){
          $scope.person = response;
          $log.debug($scope.person);
        });

        $scope.groupId = $stateParams.groupId;
        
    }
})()
