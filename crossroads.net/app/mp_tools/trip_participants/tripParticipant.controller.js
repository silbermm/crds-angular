(function(){
  'use strict()';

  module.exports = TripParticipantController;

  TripParticipantController.$inject = ['$rootScope', '$window', '$log', 'MPTools', 'Trip', 'PageInfo'];

  function TripParticipantController($rootScope, $window, $log, MPTools, Trip, PageInfo) {

    $log.debug('TripParticipantController');
    var vm = this;

    vm.cancel = cancel;
    
    vm.hasError = hasError;
    vm.groups = [];
    vm.pageInfo = PageInfo;
    vm.params = MPTools.getParams();
    vm.processing = false;
    vm.save = save;
    vm.viewReady = false;

    activate();
    //////////////////////

    function activate(){
      $log.debug('pageInfo: ' + vm.pageInfo);
      vm.errorMessages = errorMessages();
      vm.viewReady = true;
    }

    function errorMessages() {
      var errors = [];
      _.each(vm.pageInfo.errors, function(d) {
        _.each(d.messages, function(m) {
          errors.push(m);
        });
      });
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
