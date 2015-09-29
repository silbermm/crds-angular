(function() {
  'use strict()';

  module.exports = AdminGivingHistoryController;

  AdminGivingHistoryController.$inject = ['$state', 'MPTools', 'GivingHistoryService', 'AuthService', 'GIVE_ROLES'];

  function AdminGivingHistoryController($state, MPTools, GivingHistoryService, AuthService, GIVE_ROLES) {
    var vm = this;
    vm.allowAccess = allowAccess;
    vm.noSelection = undefined;
    vm.selectionError = undefined;
    vm.tooManySelections = undefined;

    activate();

    //////////////////////

    function allowAccess() {
      return (AuthService.isAuthenticated() && AuthService.isAuthorized(GIVE_ROLES.StewardshipDonationProcessor));
    };

    function activate() {
      if (!vm.allowAccess()) {
        return;
      }

      var params = MPTools.getParams();

      var donorId = getInt(params.recordId);
      if (donorId > 0) {
        goToGivingHistory(donorId);
        return;
      }

      var selectionId = getInt(params.selectedRecord);
      var selectedCount = getInt(params.selectedCount);
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

    function getInt(v) {
      var i = parseInt(v);
      return (isNaN(i) ? -1 : i);
    }
  }
})();
