(function() {
  'use strict()';

  module.exports = RecurringGivingList;

  RecurringGivingList.$inject = ['$log', '$modal', 'PaymentDisplayDetailService'];

  function RecurringGivingList($log, $modal, PaymentDisplayDetailService) {
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

      function openRemoveGiftModal(selectedDonation) {
        var modalInstance = $modal.open({
          templateUrl: 'recurring_giving_remove_modal',
          controller: 'RecurringGivingModals as recurringGift',
          resolve: {
            donation: function () {
              return selectedDonation;
            },
          }
        });

        modalInstance.result.then(function (selectedItem) {
          scope.selected = selectedItem;
        }, function () {
          $log.info('Modal dismissed at: ' + new Date());
        });
      };

      function openEditGiftModal(selectedDonation) {
        var modalInstance = $modal.open({
          templateUrl: 'recurring_giving_edit_modal',
          controller: 'RecurringGivingModals as recurringGift',
          resolve: {
            donation: function () {
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

        modalInstance.result.then(function (selectedItem) {
          scope.selected = selectedItem;
        }, function () {
          $log.info('Modal dismissed at: ' + new Date());
        });
      };
    }
  }
})();
