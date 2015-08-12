'use strict';
require('../services/group_service');
(function() {
	module.exports = function GroupSignupController($rootScope, $scope, Profile, Group, $log, $stateParams, Page, $modal) {
		$log.debug("Inside GroupSignupController");

		var vm = this;

		// if we don't have the code below, those functions would not be available in the javascript tests
		vm.editProfile = editProfile;
		vm.allSignedUp = allSignedUp;
		vm.hasParticipantID = hasParticipantID;

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
		vm.response = {};
		vm.formValid = true;

		vm.signup = function(form) {
			var test = hasParticipantID(vm.response);
			var flag = false;
			for (var i = 0; i < vm.response.length; i++) {
				if (!vm.response[i]['userInGroup'] && vm.response[i]['newAdd'] !== undefined && vm.response[i]['newAdd'] !== "") {
					flag = true;
					break;
				}
			}
			if (vm.response.length === 1) {
				flag = true;
			}

			vm.formValid = flag;
			if (!vm.formValid) {
				$rootScope.$emit('notify', $rootScope.MESSAGES.noPeopleSelectedError);
				return;
			}
			//Add Person to group
			Group.Participant.save({
				groupId: vm.groupId
			}, test).$promise.then(function(response) {
				if (vm.waitListCase) {
					$rootScope.$emit('notify', $rootScope.MESSAGES.successfullWaitlistSignup);
				} else {
					$rootScope.$emit('notify', $rootScope.MESSAGES.successfullRegistration);
				}
				vm.showContent = false;
				vm.showSuccess = true;
				vm.showWaitList = false;
				vm.showWaitSuccess = true;
			}, function(error) {
				// 422 indicates an HTTP "Unprocessable Entity", in this case meaning Group is Full
				// http://tools.ietf.org/html/rfc4918#section-11.2
				if(error.status == 422) {
					$rootScope.$emit('notify', $rootScope.MESSAGES.fullGroupError);
					vm.showFull = true;
					vm.showContent = false;
				} else {
					$rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
					vm.showFull = false;
					vm.showContent = true;
				}

			});
		};

		vm.signupPage = $rootScope.signupPage;

		// Initialize Person data for logged-in user
		Profile.Personal.get(function(response) {
			vm.person = response;
			$log.debug("Person: " + JSON.stringify(vm.person));
		});

		var pageRequest = Page.get({
			url: $stateParams.link
		}, function() {
			if (pageRequest.pages.length > 0) {
				vm.signupPage = pageRequest.pages[0];
				// retrieve group id from the CMS page
				vm.groupId = vm.signupPage.group;
				$log.debug("Group ID: " + vm.groupId);
				// Get group details
				vm.groupDetails = Group.Detail.get({
					groupId: vm.groupId
				}).$promise
					.then(function(response) {

						vm.response = response.SignUpFamilyMembers;
						if (response.waitListInd === "False" || response.waitListInd === false)
							vm.viewReady = true;
						//if(response.SignUpFamilyMembers[0].userInGroup === true){

						if (allSignedUp(response)) {
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
							// I now need to get the group-detail again for the wait list, because there are are two new possible cases
							// 1. the user is a already a member
							// 2. the user is not yet a member
							vm.groupDetails = Group.Detail.get({
								groupId: vm.groupId
							}).$promise
								.then(function(response) {
									vm.response = response.SignUpFamilyMembers;
									if (allSignedUp(response)) {
										vm.alreadySignedUp = true;
									}
									vm.viewReady = true;
								});

							//this is the case where the group is full and there is NO waitlist
						} else if ((response.groupFullInd === "True" && response.waitListInd === "False") || (response.groupFullInd === true && response.waitListInd === false)) {
							vm.showFull = true;
							vm.waitListCase = false;
							vm.showContent = false;
							vm.showWaitList = false;
							vm.viewReady = true;
							//this is the case where the group is NOT full and there IS waitlist
						} else if ((response.groupFullInd === "False" && response.waitListInd === "True") || (response.groupFullInd === false && response.waitListInd === true)) {
							vm.waitListCase = false;
							vm.showFull = false;
							vm.showContent = true;
							vm.showWaitList = false;
							vm.viewReady = true;
						}
					});

			} else {
				var notFoundRequest = Page.get({
					url: "page-not-found"
				}, function() {
					if (notFoundRequest.pages.length > 0) {
						vm.content = notFoundRequest.pages[0].content;
					} else {
						vm.content = "404 Content not found";
					}
				});
			}
		});

		function editProfile() {
			vm.modalInstance = $modal.open({
				templateUrl: 'editProfile.html',
				backdrop: true,
				// This is needed in order to get our scope
				// into the modal - by default, it uses $rootScope
				scope: $scope,
			});
		};

		function hasParticipantID(array) {
			var result = {};
			result.partId = [];
			if (array.length > 1) {
				for (var i = 0; i < array.length; i++) {
					if (array[i]['newAdd'] !== undefined && array[i]['newAdd'] !== "")
						result.partId[result.partId.length] = array[i]['newAdd'];
				}
			} else if (array.length === 1) {
				result.partId[result.partId.length] = array[0]['participantId'];
			}
			return result;
		};

		function allSignedUp(array) {
			var result = false;
			if (array.SignUpFamilyMembers.length > 1) {
				for (var i = 0; i < array.SignUpFamilyMembers.length; i++) {
					if (array.SignUpFamilyMembers[i]['userInGroup'] === false) {
						result = false;
						break;
					} else {
						result = true;
					}
				}
			} else {
				if (array.SignUpFamilyMembers[0]['userInGroup'] === false) {
					result = false;
				} else {
					result = true;
				}
			}
			return result;
		};
	}
})()
