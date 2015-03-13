'use strict';
require('../services/group_service');
(function () {
    module.exports = function GroupSignupController($scope, $rootScope, Profile, Group, $log, $stateParams) {

        $log.debug("Inside GroupSignupController");

        // Initialize Person data for logged-in user
        Profile.Personal.get(function(response){
          $scope.person = response;
          $log.debug($scope.person);
        });
        $scope.signupcalled = "Test";

        // retrieve group id from stateParams
        $scope.groupId = $stateParams.groupId;

        $scope.signup = function(){
          $log.debug("Signup clicked");

          $scope.signupcalled = true;
        };
        
    }
})()
