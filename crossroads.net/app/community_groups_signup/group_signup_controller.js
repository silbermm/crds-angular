'use strict';
require('../services/group_service');
(function () {
    module.exports = function GroupSignupController($scope, $rootScope, Profile, Group, $log, $stateParams) {

        $log.debug("Inside GroupSignupController");
		$log.debug("State params: " + JSON.stringify($stateParams));
		$log.debug($rootScope.signupPage);

    //flags to control show/hide logic
    $scope.showContent = true;
    $scope.showSuccess = false;
    $scope.showFull = false;
		
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
              $scope.showContent = false;
              $scope.showSuccess = true;
          },function(error){
              $rootScope.$emit('notify', $rootScope.MESSAGES.fullGroupError);
              $scope.showContent = false;
              $scope.showFull = true;
            });       
        };        
    }
})()
