"use strict";

(function() {

  var moment = require('moment');

  module.exports = VolunteerApplicationController;

  VolunteerApplicationController.$inject = ['$rootScope', '$scope', '$log', '$filter', 'MESSAGES', 'Session', '$stateParams', 'Profile', 'CmsInfo', '$modal', 'Opportunity'];

  function VolunteerApplicationController($rootScope, $scope, $log, $filter, MESSAGES, Session, $stateParams, Profile, CmsInfo, $modal, Opportunity) {
    $log.debug("Inside VolunteerApplicationController");
    var vm = this;

    vm.contactId = $stateParams.id;
    vm.opportunityResponse = opportunityResponse;
    vm.pageInfo = pageInfo(CmsInfo);
    vm.personalInfo = personalInfo;
    vm.save = save;
    vm.showAccessDenied = false;
    vm.showAdult = false;
    vm.showChild = false;
    vm.showContent = true;
    vm.showInvalidResponse = false;
    vm.showSuccess = false;
    vm.showBlock = showBlock;
    vm.viewReady = false;

    activate();

    ///////////////////////////////////////////

    function activate() {

      var loggedinContact = Session.exists('userId');
      if (loggedinContact !== vm.contactId) {
        vm.showAccessDenied = true;
      } else {
        vm.showAccessDenied = false;
        opportunityResponse();
        personalInfo();
      }
      vm.viewReady = true;
    }

    function opportunityResponse() {
      Opportunity.GetResponse.get({
          id: vm.pageInfo.opportunity,
          contactId: vm.contactId
        }).$promise
        .then(function(response) {
          vm.showInvalidResponse = ((response == null) || ((response.responseId == undefined)));
        });
    }

    function pageInfo(cmsInfo) {
      return cmsInfo.pages[0];
    }

    function personalInfo() {
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

    function save(form) {

    }

    function showBlock(blockName) {
      switch (blockName) {
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
