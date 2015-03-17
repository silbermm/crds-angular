'use strict';
require('../services/group_service');
(function () {
    module.exports = function GroupSignupController($scope, $rootScope, Profile, Group, $log, $stateParams, Page) {
        $log.debug("Inside GroupSignupController");

        //flags to control show/hide logic
        $scope.showContent = true;
        $scope.showSuccess = false;
        $scope.showFull = false;
        
        $scope.signupPage = $rootScope.signupPage;

        // Initialize Person data for logged-in user
        Profile.Personal.get(function(response){
            $scope.person = response;
            $log.debug("Person: " + $scope.person);
        });
            
        var pageRequest = Page.get({ url: $stateParams.urlsegment }, function() {
            if (pageRequest.pages.length > 0) {
                $scope.signupPage = pageRequest.pages[0];
                // retrieve group id from the CMS page
                $scope.groupId = $scope.signupPage.group;
                $log.debug("Group ID: " + $scope.groupId);
            } else {
				$log.debug("Group page not found for " + $stateParams.urlsegment);
                var notFoundRequest = Page.get({ url: "page-not-found" }, function() {
                    if (notFoundRequest.pages.length > 0) {
                        $scope.content = notFoundRequest.pages[0].renderedContent;
                    } else {
                        $scope.content = "404 Content not found";
                    }
                });
            }
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
