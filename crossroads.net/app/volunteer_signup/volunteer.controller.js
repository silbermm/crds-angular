"use strict";

(function() {

  module.exports = VolunteerController;

  VolunteerController.$inject = ['$rootScope', '$scope', '$log', '$filter', 'MESSAGES', 'Session', 'Opportunity', 'ServeOpportunities', 'CmsInfo', '$modal'];

  function VolunteerController($rootScope, $scope, $log, $filter, MESSAGES, Session, Opportunity, ServeOpportunities, CmsInfo, $modal) {
    $log.debug("Inside VolunteerController");
    var vm = this;

    vm.allSignedUp = allSignedUp;
    vm.disableCheckbox = disableCheckbox;
    vm.displayEmail = displayEmail;
    vm.displayPendingFlag = displayPendingFlag;
    vm.editProfile = editProfile;
    vm.modalInstance = {};
    vm.pageInfo = pageInfo(CmsInfo);
    vm.participants = null;
    vm.save = save;
    vm.showAllSignedUp = false;
    vm.showContent = true;
    vm.showSuccess = false;
    vm.viewReady = false;

    init();

    function allSignedUp() {
      var signupCount = 0;
      _.each(vm.participants, function(p){
        if (p.memberOfGroup || p.pending) {
          signupCount = signupCount + 1;
        }
      });
      if (signupCount === vm.participants.length) {
        vm.showAllSignedUp = true;
        vm.showContent = false;
      }
    }

    function disableCheckbox(participant) {
      if (participant.memberOfGroup || participant.pending) {
        return true;
      }
      return false;
    }

    function displayEmail(emailAddress) {
      if (emailAddress === null || emailAddress === undefined) {
        return false;
      }
      if (emailAddress.length > 0) {
        return true;
      }
      return false;
    }

    function displayPendingFlag(participant) {
      if (participant.memberOfGroup) {
        return false;
      }
      if (participant.pending) {
        return true;
      }
      return false;
    }

    function editProfile() {
			vm.modalInstance = $modal.open({
				templateUrl: 'editProfile.html',
				backdrop: true,
				// This is needed in order to get our scope
				// into the modal - by default, it uses $rootScope
				scope: $scope,
			});
		}

    function init() {
      ServeOpportunities.QualifiedServers.query({
          groupId: vm.pageInfo.group,
          contactId: Session.exists('userId')
        }).$promise
        .then(function(response) {
          vm.participants = response;
          allSignedUp();
          vm.viewReady = true;
        });
    }

    function pageInfo(cmsInfo) {
      // TODO need to check that we have data before assign
      // can we check for 404 on route?  and assume we have a page?
      return cmsInfo.pages[0];
    }

    function save(form) {
      var save = new ServeOpportunities.SaveQualifiedServers();
      save.opportunityId = vm.pageInfo.opportunity;
      //just get participants that have checkbox checkLoggedin
      save.participants = _.pluck(_.filter(vm.participants, {
        add: true
      }), 'participantId');

      $log.debug(save.participants.length);
      if (save.participants.length < 1) {
        $rootScope.$emit('notify', $rootScope.MESSAGES.noPeopleSelectedError);
        return;
      }

      save.$save().then(function() {
        vm.created = true;
        vm.showContent = false;
        vm.showSuccess = true;
      }, function() {
        vm.rejected = true;
      });
    }
  }
})();
