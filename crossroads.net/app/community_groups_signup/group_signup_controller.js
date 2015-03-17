'use strict';
require('../services/group_service');
(function () {
    module.exports = function GroupSignupController($scope, $rootScope, Profile, Group, $log, $stateParams) {

        $log.debug("Inside GroupSignupController");
		$log.debug("State params: " + JSON.stringify($stateParams));
		$log.debug("$rootScope.signupPage: " + JSON.stringify($rootScope.signupPage));
		
		$scope.signupPage = $rootScope.signupPage;

        // Initialize Person data for logged-in user
        Profile.Personal.get(function(response){
          $scope.person = response;
          $log.debug($scope.person);
        });

        // retrieve group id from stateParams
        $log.debug($scope.groupId = $stateParams.groupId);

        $scope.signup = function(){
          //Add Person to group
          Group.save().$promise.then(function(response) {
              $rootScope.$emit('notify', $rootScope.MESSAGES.successfullRegistration);
          },function(error){
              $rootScope.$emit('notify', $rootScope.MESSAGES.fullGroupError);
            });       
        };        
    }
})()
