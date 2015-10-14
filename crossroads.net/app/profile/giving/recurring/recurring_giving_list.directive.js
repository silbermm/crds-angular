(function() {
  'use strict()';

  module.exports = RecurringGivingList;

  RecurringGivingList.$inject = ['$rootScope', '$log', '$modal', 'PaymentDisplayDetailService'];

  function RecurringGivingList($rootScope, $log, $modal, PaymentDisplayDetailService) {
    return {
      restrict: 'EA',
      transclude: true,
      templateUrl: 'templates/recurring_giving_list.html',
      scope: {
        recurringGiftsInput: '=',
      },
      link: link
    };

    function link(scope) {
      scope.openRemoveGiftModal = openRemoveGiftModal;
      scope.openEditGiftModal = openEditGiftModal;

      scope.$watch('recurringGiftsInput', function(recurringGifts) {
        scope.recurringGifts = PaymentDisplayDetailService.postProcess(recurringGifts);
      });

      function openRemoveGiftModal(selectedDonation, index) {
        var modalInstance = $modal.open({
          parent: 'noSideBar',
          templateUrl: 'recurring_giving_remove_modal',
          controller: 'RecurringGivingModals as recurringGift',
          resolve: {
            donation: function() {
              return selectedDonation;
            },

            programList: function() {
              return [
                {
                  ProgramId: selectedDonation.program,
                  Name: selectedDonation.program_name,
                }
              ];
            },
          }
        });

        modalInstance.result.then(function(success) {
          if (success) {
            scope.recurringGifts.splice(index, 1);
            $rootScope.$emit('notify', $rootScope.MESSAGES.giveRecurringRemovedSuccess);
          } else {
            $rootScope.$emit('notify', $rootScope.MESSAGES.failedResponse);
          }
        }, function() {

          $log.info('Modal dismissed at: ' + new Date());
        });
      };

      function openEditGiftModal(selectedDonation) {
        var modalInstance = $modal.open({
          parent: 'noSideBar',
          templateUrl: 'recurring_giving_edit_modal',
          controller: 'RecurringGivingModals as recurringGift',
          resolve: {
            donation: function() {
              return selectedDonation;
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

        modalInstance.result.then(function(selectedItem, success) {
          scope.selected = selectedItem;
        }, function() {

          $log.info('Modal dismissed at: ' + new Date());
        });
      };
    }
  }
})();
