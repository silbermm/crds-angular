'use strict';
require('../services/group_service');
(function () {
    module.exports = function GroupSignupController($rootScope, Profile, Group, $log, $stateParams, Page) {
        $log.debug("Inside GroupSignupController");

        var vm = this;

        //flags to control show/hide logic
        vm.showContent = true;
        vm.showSuccess = false;
        vm.showFull = false;
        
        // Initialize Person data for logged-in user
        Profile.Personal.get(function(response){
            vm.person = response;
            $log.debug("Person: " + vm.person);
        });
            
        var pageRequest = Page.get({ url: $stateParams.urlsegment }, function() {
            if (pageRequest.pages.length > 0) {
                vm.signupPage = pageRequest.pages[0];
                // retrieve group id from the CMS page
                vm.groupId = vm.signupPage.group;
                $log.debug("Group ID: " + vm.groupId);
            } else {
				$log.debug("Group page not found for " + $stateParams.urlsegment);
                var notFoundRequest = Page.get({ url: "page-not-found" }, function() {
                    if (notFoundRequest.pages.length > 0) {
                        vm.content = notFoundRequest.pages[0].renderedContent;
                    } else {
                        vm.content = "404 Content not found";
                    }
                });
            }
        });

        vm.signup = function(){
            //Add Person to group
            Group.save({groupId : vm.groupId}).$promise.then(function(response) {
                $rootScope.$emit('notify', $rootScope.MESSAGES.successfullRegistration);
                vm.showContent = false;
                vm.showSuccess = true;
            },function(error){
                $rootScope.$emit('notify', $rootScope.MESSAGES.fullGroupError);
                vm.showContent = false;
                vm.showFull = true;
            });       
        };        
    }
})()
