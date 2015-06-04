"use strict";

(function() {

  var moment = require('moment');

  module.exports = VolunteerApplicationController;

  VolunteerApplicationController.$inject = ['$rootScope', '$scope', '$log', '$filter', 'MESSAGES', 'Session', '$stateParams', 'Profile', 'CmsInfo', '$modal', 'Opportunity'];

  function VolunteerApplicationController($rootScope, $scope, $log, $filter, MESSAGES, Session, $stateParams, Profile, CmsInfo, $modal, Opportunity) {
    $log.debug("Inside VolunteerApplicationController");
    var vm = this;

    vm.allSignedUp = allSignedUp;
    vm.allowSubmission = true;
    vm.contactId = $stateParams.id;
    vm.disableCheckbox = disableCheckbox;
    vm.displayEmail = displayEmail;
    vm.displayPendingFlag = displayPendingFlag;
    vm.editProfile = editProfile;
    vm.modalInstance = {};
    vm.pageInfo = pageInfo(CmsInfo);
    vm.participants = null;
    vm.save = save;
    vm.showAdult = false;
    vm.showAllSignedUp = false;
    vm.showChild = false;
    vm.showContent = true;
    vm.showSuccess = false;
    vm.viewReady = false;

    activate();

    ///////////////////////////////////////////

    function activate() {


      // $log.debug('contact id: '+vm.contactId);
      // $log.debug('state: ' + $stateParams);
      //does this person have a valid response?
      Opportunity.GetResponse.get({
          id: vm.pageInfo.opportunity,
          contactId: vm.contactId
        }).$promise
        .then(function(response) {
          $log.debug("Opportunity Response");
          var tmp = response;
          var id = response.responseId;
          vm.allowSubmission = ((response !== null) && ((response.responseId !== undefined)));
          $log.debug('allowSubmission: '+vm.allowSubmission);
        });

      // Initialize Person data for logged-in user
      Profile.Personal.get(function(response) {
        vm.person = response;
        $log.debug("Person: " + JSON.stringify(vm.person));
        // vm.age = moment(vm.person.dateOfBirth, "MM/DD/YYYY").fromNow().split(" ")[0];
        //vm.age = moment().diff(moment(vm.person.dateOfBirth, 'MM/DD/YYYY'), 'years')
        //$log.debug('age: '+vm.age);

        if (vm.person.age >= 16) {
          vm.showAdult = true;
        } else if ((vm.person.age >= 14) && (vm.person.age <= 15)) {
          vm.showChild = true;
        } else {
          vm.showError = true;
        }
        $log.debug('showAdult: '+vm.showAdult);
      });

      // ServeOpportunities.QualifiedServers.query({
      //     groupId: vm.pageInfo.group,
      //     contactId: Session.exists('userId')
      //   }, function(response) {
      //     vm.participants = response;
      //     allSignedUp();
      //     vm.viewReady = true;
      //   }, function(err){
      //     $state.go('content', {link:'/server-error/'});
      //   });

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
  }
})();
