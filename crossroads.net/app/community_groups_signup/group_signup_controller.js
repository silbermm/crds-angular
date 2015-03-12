'use strict';
(function () {
    module.exports = function GroupSignupController($scope, $rootScope, Profile, $log) {
        $log.debug("Inside GroupSignupController");
        Profile.Personal.get(function(response){
          $scope.person = response;
        });

       

       
    }
})()
