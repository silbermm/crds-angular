'use strict';
require('../services/group_service');
(function () {
    module.exports = function GroupSignupController($rootScope, $scope, Profile, Group, $log, $stateParams, Page, $modal) {
        $log.debug("Inside GroupSignupController");

        var vm = this;

        vm.editProfile = editProfile;

        //flags to control show/hide logic
        vm.showContent = true;
        vm.showSuccess = false;
        vm.showFull = false;
        vm.showWaitList = false;
        vm.showWaitSuccess = false;
        vm.waitListCase = false;
        vm.alreadySignedUp = false;
        vm.viewReady = false;
        vm.modalInstance = {};

        vm.signupPage = $rootScope.signupPage;

        // Initialize Person data for logged-in user
        Profile.Personal.get(function (response) {
            vm.person = response;
            $log.debug("Person: " + JSON.stringify(vm.person));
        });

        var pageRequest = Page.get({
            url: $stateParams.link
        }, function () {
            if (pageRequest.pages.length > 0) {
                vm.signupPage = pageRequest.pages[0];
                // retrieve group id from the CMS page
                vm.groupId = vm.signupPage.group;
                $log.debug("Group ID: " + vm.groupId);
                // Get group details
                vm.groupDetails = Group.Detail.get({
                        groupId: vm.groupId
                    }).$promise
                    .then(function (response) {
                    vm.viewReady = true;
                    if(response.userInGroup === true){
                        vm.alreadySignedUp = true;
                    }
                        // This is the case where the group is full and there is a waitlist
                        if ((response.groupFullInd === "True" && response.waitListInd === "True") || (response.groupFullInd === true && response.waitListInd === true)) {
                            vm.waitListCase = true;
                            vm.showWaitList = true;
                            //append "- WaitList" to title
                            vm.signupPage.title = vm.signupPage.title + " - Waitlist";
                            //update groupID to waitList ID
                            vm.groupId = response.waitListGroupId;
                            //this is the case where the group is full and there is NO waitlist
                        } else if ((response.groupFullInd === "True" && response.waitListInd === "False") || (response.groupFullInd === true && response.waitListInd === false)) {
                            vm.showFull = true;
                            vm.showContent = false;

                        }
                    });

            } else {
                var notFoundRequest = Page.get({
                    url: "page-not-found"
                }, function () {
                    if (notFoundRequest.pages.length > 0) {
                        vm.content = notFoundRequest.pages[0].renderedContent;
                    } else {
                        vm.content = "404 Content not found";
                    }
                });
            }
        });

        vm.signup = function () {
            //Add Person to group
            Group.Participant.save({
                groupId: vm.groupId
            }).$promise.then(function (response) {
                $rootScope.$emit('notify', $rootScope.MESSAGES.successfullRegistration);
                vm.showContent = false;
                vm.showSuccess = true;
                vm.showWaitList = false;
                vm.showWaitSuccess = true;
            }, function (error) {
                $rootScope.$emit('notify', $rootScope.MESSAGES.fullGroupError);
                vm.showContent = false;
                vm.showFull = true;
            });
        };

        function editProfile() {
            vm.modalInstance = $modal.open({
                templateUrl: 'editProfile.html',
                backdrop: true,
                // This is needed in order to get our scope
                // into the modal - by default, it uses $rootScope
                scope: $scope,
            });
        };
    }
})()
