(function(){
  'use strict()';

  module.exports = TripParticipantController;

  TripParticipantController.$inject = ['$rootScope', '$log', 'MPTools', 'Trip', 'PageInfo'];

  function TripParticipantController($rootScope, $log, MPTools, Trip, PageInfo) {

    $log.debug('TripParticipantController');
    var vm = this;
    vm.errorMessage = $rootScope.MESSAGES.toolsError;
    vm.groups = [];
    vm.pageInfo = PageInfo;
    vm.params = MPTools.getParams();
    // vm.person = Contact;
    vm.selectedRecord = null;
    vm.showError = showError;
    vm.showSuccess = false;
    // vm.viewReady = false;
    vm.viewReady = true;

    activate();
    //////////////////////

    function activate(){
      $log.debug('pageInfo: ' + vm.pageInfo);
      // $log.debug('selectedCount: ' + vm.params.selectedCount);

      // Trip.TripFormResponses.query(
      //   {selectionId: vm.params.selectedRecord, selectionCount: vm.params.selectedCount})
      // .$promise
      // .then(function(response){
      //   $log.debug(response);
      // });
    }

    

    function showError(){
      return false;
      // if (vm.params.selectedCount > 1 || vm.params.recordDescription === undefined || vm.params.recordId === '-1'){
      //   vm.errorMessage = $rootScope.MESSAGES.toolsError;
      //   vm.error = true;
      // } 
      // return vm.error;
    }
  }

})();
