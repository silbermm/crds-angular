(function() {
  'use strict()';

  module.exports = RecurringGivingList;

  RecurringGivingList.$inject = ['$log', 'PaymentDisplayDetailService'];

  function RecurringGivingList($log, PaymentDisplayDetailService) {
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
      scope.$watch('recurringGiftsInput', function(recurringGifts) {
        scope.recurringGifts = PaymentDisplayDetailService.postProcess(recurringGifts);
      });
    }
  }
})();
