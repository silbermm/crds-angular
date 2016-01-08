(function() {
  'use strict()';

  module.exports = TripParticipantController;

  TripParticipantController.$inject = ['$rootScope', '$window', '$log', 'MPTools', 'Trip', 'PageInfo', 'AuthService', 'CRDS_TOOLS_CONSTANTS'];

  function TripParticipantController($rootScope, $window, $log, MPTools, Trip, PageInfo, AuthService, CRDS_TOOLS_CONSTANTS) {

    $log.debug('TripParticipantController');
    var vm = this;

    vm.allowAccess = allowAccess;
    vm.cancel = cancel;
    vm.hasError = hasError;
    vm.groups = [];
    vm.pageInfo = PageInfo;
    vm.params = MPTools.getParams();
    vm.processing = false;
    vm.save = save;
    vm.selectionMessage = '';
    vm.viewReady = false;

    activate();

    //////////////////////

    function allowAccess() {
      return (AuthService.isAuthenticated() && AuthService.isAuthorized(CRDS_TOOLS_CONSTANTS.SECURITY_ROLES.TripTools));
    }

    function selectionMessage() {
      if (vm.params.recordId === '-1') {
        vm.selectionMessage = 'Your selection contained ' + vm.params.selectedCount + ' member(s).';
      }
    }

    function activate() {
      $log.debug('pageInfo: ' + vm.pageInfo);
      vm.errorMessages = errorMessages();
      vm.viewReady = true;
      selectionMessage();
    }

    function errorMessages() {
      var errors = [];
      _.each(vm.pageInfo.errors, function(d) {
        errors.push(d.message);
      });

      if (vm.pageInfo.data !== undefined) {
        _.each(vm.pageInfo.data.errors, function(d) {
          errors.push(d);
        });
      }

      return errors;
    }

    function hasError() {
      if (vm.errorMessages.length > 0) {
        return true;
      }

      return false;
    }

    function cancel() {
      $window.close();
    }

    function save() {
      vm.processing = true;
      var dto = {};

      dto.applicants = vm.pageInfo.applicants;
      dto.group = vm.group.groupId;
      dto.pledgeCampaign = vm.pageInfo.campaign;

      Trip.SaveParticipants.save(dto, function(updatedEvents) {
        $window.close();
      }, function(err) {

        $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
        vm.processing = false;
      });

    }
  }

})();
