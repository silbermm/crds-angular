(function(){
  'use strict()';

  module.exports = TripParticipantController;

  TripParticipantController.$inject = ['$rootScope', '$window', '$log', 'MPTools', 'Trip', 'PageInfo'];

  function TripParticipantController($rootScope, $window, $log, MPTools, Trip, PageInfo) {

    $log.debug('TripParticipantController');
    var vm = this;
    vm.errorMessage = $rootScope.MESSAGES.toolsError;
    vm.groups = [];
    vm.pageInfo = PageInfo;
    vm.params = MPTools.getParams();
    vm.save = save;
    vm.viewReady = false;

    activate();
    //////////////////////

    function activate(){
      $log.debug('pageInfo: ' + vm.pageInfo);
      vm.viewReady = true;
    }

    function save() {
      var dto = {};

      dto.applicants = vm.pageInfo.applicants;
      dto.group = vm.group.groupId;
      dto.pledgeCampaign = vm.pageInfo.campaign;

      Trip.SaveParticipants.save(dto, function(updatedEvents) {
        $window.close();
      }, function(err) {
        $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
      });
    }
  }

})();
