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


      scope.items = ['item1', 'item2', 'item3'];
      function openRemoveGiftModal(size) {
        var modalInstance = $modal.open({
          templateUrl: 'recurring_giving_remove_modal',
          controller: 'ModalInstanceCtrl',
          size: size,
          resolve: {
            items: function () {
              return scope.items;
            }
          }
        });

        modalInstance.result.then(function (selectedItem) {
          scope.selected = selectedItem;
        }, function () {
          $log.info('Modal dismissed at: ' + new Date());
        });
      };

      function openEditGiftModal(size) {
        var modalInstance = $modal.open({
          templateUrl: 'recurring_giving_edit_modal',
          controller: 'ModalInstanceCtrl',
          size: size,
          resolve: {
            items: function () {
              return scope.items;
            }
          }
        });

        modalInstance.result.then(function (selectedItem) {
          $scope.selected = selectedItem;
        }, function () {
          $log.info('Modal dismissed at: ' + new Date());
        });
      };
    }
  }
})();
