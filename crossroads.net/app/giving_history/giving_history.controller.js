(function() {
  'use strict';
  module.exports = GivingHistoryController;

  GivingHistoryController.$inject = ['GivingHistoryService'];

  function GivingHistoryController(GivingHistoryService) {
    var vm = this;

    vm.donations = [];

    activate();

    function activate() {
      GivingHistoryService.donations.query(function(data) {
        vm.donations = data;
      });
    }

  }
})();
