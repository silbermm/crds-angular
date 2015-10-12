(function() {
  'use strict';

  module.exports = RecurringGivingModals;

  RecurringGivingModals.$inject = ['$modalInstance', 'RecurringGivingService', 'donation'];

  function RecurringGivingModals($modalInstance, RecurringGivingService, donation) {
    var vm = this;
    vm.donation = donation;
  };

})();
