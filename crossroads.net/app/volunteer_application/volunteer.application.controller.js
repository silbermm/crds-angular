"use strict";

(function() {

  var moment = require('moment');

  module.exports = VolunteerApplicationController;

  VolunteerApplicationController.$inject = ['$rootScope', '$scope', '$log', '$filter', 'MESSAGES', 'Session', '$stateParams', 'Profile', 'CmsInfo', '$modal', 'Opportunity'];

  function VolunteerApplicationController($rootScope, $scope, $log, $filter, MESSAGES, Session, $stateParams, Profile, CmsInfo, $modal, Opportunity) {
    $log.debug("Inside VolunteerApplicationController");
    var vm = this;

    vm.allSignedUp = allSignedUp;
    //vm.allowSubmission = true;
    vm.contactId = $stateParams.id;
    vm.disableCheckbox = disableCheckbox;
    vm.displayEmail = displayEmail;
    vm.displayPendingFlag = displayPendingFlag;
    vm.editProfile = editProfile;
    vm.modalInstance = {};

    vm.pageInfo = pageInfo(CmsInfo);
    vm.participants = null;
    vm.save = save;
    vm.showAccessDenied = false;
    vm.showAdult = false;
    vm.showAllSignedUp = false;
    vm.showChild = false;
    vm.showContent = true;
    vm.showInvalidResponse = false;
    vm.showSuccess = false;
    vm.show = show;
    vm.validResponse = false;
    vm.viewReady = false;

    activate();

    ///////////////////////////////////////////

    function activate() {

      var loggedinContact = Session.exists('userId');
      if (loggedinContact !== vm.contactId) {
        vm.showAccessDenied = true;
      } else {
        vm.showAccessDenied = false;

        Opportunity.GetResponse.get({
            //id: 116,
            id: vm.pageInfo.opportunity,
            contactId: vm.contactId
          }).$promise
          .then(function(response) {
            $log.debug("Opportunity Response");
            vm.showInvalidResponse = ((response == null) || ((response.responseId == undefined)));
            $log.debug('showInvalidResponse: ' + vm.showInvalidResponse);
          });

        // Initialize Person data for logged-in user
        Profile.Personal.get(function(response) {
          vm.person = response;
          $log.debug("Person: " + JSON.stringify(vm.person));

          if (vm.person.age >= 16) {
            vm.showAdult = true;
          } else if ((vm.person.age >= 14) && (vm.person.age <= 15)) {
            vm.showChild = true;
          } else {
            vm.showError = true;
          }
          $log.debug('showAdult: ' + vm.showAdult);
        });
      }
      vm.viewReady = true;
    }

    function allSignedUp() {
      var signupCount = 0;
      _.each(vm.participants, function(p) {
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

    function editProfile(personToEdit) {
      vm.modalInstance = $modal.open({
        templateUrl: 'profile/editProfile.html',
        backdrop: true,
        controller: "ProfileModalController as modal",
        // This is needed in order to get our scope
        // into the modal - by default, it uses $rootScope
        scope: $scope,
        resolve: {
          person: function() {
            return personToEdit;
          }
        }
      });
      vm.modalInstance.result.then(function(person) {
        personToEdit.preferredName = person.nickName === null ? person.firstName : person.nickName;
        $rootScope.$emit("personUpdated", person);
      });
    }


    function pageInfo(cmsInfo) {
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

    function show(block) {
      switch (block) {
        case 'adult':
          return vm.showAdult && !vm.showInvalidResponse;
          break;
        case 'child':
          return vm.showChild && !vm.showInvalidResponse;
          break;
        case 'no-response':
          return vm.showInvalidResponse;
          break;
        case 'denied':
          return vm.showAccessDenied;
          break;
        case 'age-error':
          return vm.showError;
          break;
        default:
          $log.debug('show block undefined: ' + block);
          return false;
      }
    }
  }
})();
