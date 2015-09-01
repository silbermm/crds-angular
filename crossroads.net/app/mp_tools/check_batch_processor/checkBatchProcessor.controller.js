(function() {
  'use strict()';

  module.exports = CheckBatchProcessor;

  CheckBatchProcessor.$inject = ['$rootScope', '$log', 'MPTools', 'CheckScannerBatches', 'getPrograms', 'AuthService', 'GIVE_PROGRAM_TYPES', 'GIVE_ROLES'];

  function CheckBatchProcessor($rootScope, $log, MPTools, CheckScannerBatches, getPrograms, AuthService, GIVE_PROGRAM_TYPES, GIVE_ROLES) {
    var vm = this;

    vm.batch = {};
    vm.batches = [];
    vm.programs = [];
    vm.program = {};
    vm.processing = false;
    vm.params = MPTools.getParams();

    activate();
    //////////////////////

    function activate() {
      CheckScannerBatches.query(function(data) {
        vm.batches = data;
      });

      getPrograms.Programs.get({'excludeTypes[]': [GIVE_PROGRAM_TYPES.NonFinancial]}, function(data) {
        vm.programs = _.sortBy(data, 'Name');
      });
    }

    vm.allowAccess = function() {
      return(AuthService.isAuthenticated() && AuthService.isAuthorized(GIVE_ROLES.StewardshipDonationProcessor));
    }

    vm.processBatch = function() {
      vm.processing = true;

      CheckScannerBatches.save({name: vm.batch.name, programId: vm.program.ProgramId}).$promise.then(function(){
          vm.success = true;
          vm.error = false;
        }, function(){
          vm.success = false;
          vm.error=true;
      }).finally(function(){
        vm.processing = false;
      });
    };
  }
})();
