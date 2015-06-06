"use strict";

(function() {

  var moment = require('moment');

  module.exports = VolunteerApplicationController;

  VolunteerApplicationController.$inject = ['$scope', '$log', 'MESSAGES', 'Session', '$stateParams', 'CmsInfo', 'Opportunity', 'Contact'];

  function VolunteerApplicationController($scope, $log, MESSAGES, Session, $stateParams, CmsInfo, Opportunity, Contact) {
    $log.debug("Inside VolunteerApplicationController");
    var vm = this;

    vm.contactId = $stateParams.id;
    vm.pageInfo = pageInfo(CmsInfo);
    vm.person = Contact;
    // responseCheck, need a better way to handle
    // we display form before OpportunityReponse is
    // returned.  When it returns false, form is hidden
    // and proper message display, but view flashes
    vm.responseCheck = false;
    vm.save = save;
    vm.showAccessDenied = false;
    vm.showAdult = false;
    vm.showContent = true;
    vm.showInvalidResponse = false;
    vm.showStudent = false;
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
        applicationVersion();
      }
      vm.viewReady = true;
    }

    function applicationVersion() {
      $log.debug("Person: " + JSON.stringify(vm.person));

      if (vm.person.age >= 16) {
        vm.showAdult = true;
      } else if ((vm.person.age >= 14) && (vm.person.age <= 15)) {
        vm.showStudent = true;
      } else {
        vm.showError = true;
      }
    }

    function opportunityResponse() {
      Opportunity.GetResponse.get({
          id: vm.pageInfo.opportunity,
          contactId: vm.contactId
        }).$promise
        .then(function(response) {
          vm.responseCheck = true;
          vm.showInvalidResponse = ((response == null) || ((response.responseId == undefined)));
        });
    }

    function pageInfo(cmsInfo) {
      return cmsInfo.pages[0];
    }

    function save(form) {

    }

    function showBlock(blockName) {
      switch (blockName) {
        case 'adult':
          vm.showContent = true;
          return vm.showAdult && !vm.showInvalidResponse && vm.responseCheck;
          break;
        case 'student':
          vm.showContent = true;
          return vm.showStudent && !vm.showInvalidResponse && vm.responseCheck;
          break;
        case 'no-response':
          vm.showContent = false;
          return vm.showInvalidResponse;
          break;
        case 'denied':
          vm.showContent = false;
          return vm.showAccessDenied;
          break;
        case 'age-error':
          vm.showContent = false;
          return vm.showError;
          break;
        default:
          $log.debug('show block undefined: ' + block);
          return false;
      }
    }
  }
})();
