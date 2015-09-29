(function() {
  'use strict()';

  module.exports = AdminGivingHistoryController;

  AdminGivingHistoryController.$inject = ['$state', 'MPTools', 'GivingHistoryService', 'AuthService', 'GIVE_ROLES'];

  function AdminGivingHistoryController($state, MPTools, GivingHistoryService, AuthService, GIVE_ROLES) {
    var vm = this;
    vm.noSelection = undefined;
    vm.selectionError = undefined;
    vm.tooManySelections = undefined;

    activate();

    //////////////////////

    vm.allowAccess = function() {
      return (AuthService.isAuthenticated() && AuthService.isAuthorized(GIVE_ROLES.StewardshipDonationProcessor));
    };

    function activate() {
      var params = MPTools.getParams();

      var donorId = parseInt(params.recordId);
      if (donorId > 0) {
        goToGivingHistory(donorId);
        return;
      }

      var selectionId = parseInt(params.selectedRecord);
      var selectedCount = parseInt(params.selectedCount);
      if (selectedCount == 1 && selectionId > 0) {
        MPTools.Selection.get({selectionId: selectionId}, function(data) {
          goToGivingHistory(data.RecordIds[0]);
        }, function(error) {
          vm.selectionError = true;
        });
      }

      vm.noSelection = donorId <= 0 && selectionId <= 0;
      vm.tooManySelections = selectedCount > 1;
    }

    function goToGivingHistory(donorId) {
      GivingHistoryService.impersonateDonorId = donorId;
      $state.go('tools.adminGivingHistory');
    }
  }
})();
