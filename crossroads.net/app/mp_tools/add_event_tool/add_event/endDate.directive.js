(function() {
  'use strict';

  module.exports = EndDate;

  EndDate.$inject = [];

  function EndDate() {
    return {
      restrict: 'A',
      require: 'ngModel',
      link: function(scope, elm, attrs, ctrl) {
        ctrl.$validators.endDate = function(value) {
          return value.getTime() >= scope.evt.eventData.startDate.getTime()
        };
      }
    };
  }
})();
