(function() {
  'use strict';

  module.exports = AdminRecurringGiftController;

  AdminRecurringGiftController.$inject = ['$log', '$filter', '$state', '$modal', '$rootScope', 'DonationService', 'GiveTransferService'];

  function AdminRecurringGiftController($log, $filter, $state, $modal, $rootScope, DonationService, GiveTransferService) {
    var vm = this;
    vm.recurring_gifts = [];
    vm.recurring_giving = false;
    vm.recurring_giving_view_ready = false;
    vm.openCreateGiftModal = openCreateGiftModal;
    vm.modalInstance = undefined;
    vm.impersonateDonorId = undefined;

    activate();

    function activate() {
      vm.impersonateDonorId = GiveTransferService.impersonateDonorId;

      DonationService.queryRecurringGifts(vm.impersonateDonorId).then(function(data) {
        vm.recurring_gifts = data;
        vm.recurring_giving_view_ready = true;
        vm.recurring_giving = true;
      }, function(/*error*/) {

        vm.recurring_giving = false;
        vm.recurring_giving_view_ready = true;
      });
    }

    function openCreateGiftModal() {
      vm.modalInstance = $modal.open({
        parent: 'noSideBar',
        templateUrl: 'recurring_giving_create_modal',
        controller: 'RecurringGivingModals as recurringGift',
        resolve: {
          donation: function() {
            return null;
          },

          Programs: 'Programs',
          programList: function(Programs) {
            // TODO The number one relates to the programType in MP. At some point we should fetch
            // that number from MP based in human readable input here.
            return Programs.Programs.query({
              programType: 1
            }).$promise;
          },
        }
      });

      vm.modalInstance.result.then(function(success) {
        if (success) {
          DonationService.queryRecurringGifts(vm.impersonateDonorId).then(function(data) {
            vm.recurring_gifts = data;
          }, function(/*error*/) {

            $rootScope.$emit('notify', $rootScope.MESSAGES.failedResponse);
          });

          $rootScope.$emit('notify', $rootScope.MESSAGES.giveRecurringSetupSuccess);
        } else {
          $rootScope.$emit('notify', $rootScope.MESSAGES.giveRecurringSetupWarning);
        }
      }, function() {

        $log.info('Modal dismissed at: ' + new Date());
      });
    }
  }
})();
