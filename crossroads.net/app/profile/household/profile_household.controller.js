'use strict';
(function() {
  module.exports = ProfileHouseholdController;

  ProfileHouseholdController.$inject = ['$log'];

  function ProfileHouseholdController($log) {
    var vm = this;

    $log.debug('householdId: ' + vm.householdId);
  }
})();
