(function() {
  'use strict()';

  module.exports = AdminGivingHistoryController;

  AdminGivingHistoryController.$inject = ['$state', 'MPTools', 'GivingHistoryService', 'GIVE_ROLES'];

  function AdminGivingHistoryController($state, MPTools, GivingHistoryService, GIVE_ROLES) {
    var vm = this;
    vm.service = MPTools;
    vm.allowAccess = undefined;

    activate();

    //////////////////////

    function activate() {
      vm.allowAccess = vm.service.allowAccess(GIVE_ROLES.StewardshipDonationProcessor);
      if (!vm.allowAccess) {
        return;
      }

      MPTools.getDonorId(goToGivingHistory);
    }

    function goToGivingHistory(donorId) {
      GivingHistoryService.impersonateDonorId = donorId;
      $state.go('tools.adminGivingHistory');
    }
  }
})();
