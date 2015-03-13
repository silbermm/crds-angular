'use strict';
require('../services/group_service');
(function () {
    module.exports = function GroupSignupController($scope, $rootScope, Profile, Group, $log, $stateParams) {
        _this = this;

        $log.debug("Inside GroupSignupController");

        // Initialize Person data for logged-in user
        Profile.Personal.get(function(response){
          _this.person = response;
          $log.debug($scope.person);
        });
        _this.signupcalled = "Test";

        // retrieve group id from stateParams
        _this.groupId = $stateParams.groupId;

        $scope.signup = function(){
          $log.debug("Signup clicked");

          _this.signupcalled = true;
        };
        
    }
})()
