(function() {
  'use strict';

  module.exports = RecurringGivingService;

  RecurringGivingService.$inject = ['$resource', '$modal', '$log'];

  function RecurringGivingService($resource, $modal, $log) {
    var recurringGivingService = {
      recurringGifts: $resource(__API_ENDPOINT__ + 'api/donor/recurrence'),
    };

    function openRemoveGiftModal(size) {
      var modalInstance = $modal.open({
        templateUrl: 'recurring_giving_remove_modal',
        controller: 'ModalInstanceCtrl',
        size: size,
        resolve: {
          items: function () {
            return $scope.items;
          }
        }
      });

      modalInstance.result.then(function (selectedItem) {
        $scope.selected = selectedItem;
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
            return $scope.items;
          }
        }
      });

      modalInstance.result.then(function (selectedItem) {
        $scope.selected = selectedItem;
      }, function () {
        $log.info('Modal dismissed at: ' + new Date());
      });
    };

    return recurringGivingService;
  }

})();
