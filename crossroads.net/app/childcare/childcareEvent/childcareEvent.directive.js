(function() {
  'use strict';

  module.exports = ChildcareEvent;

  ChildcareEvent.$inject = [];

  function ChildcareEvent() {
    return {
      restrict: 'E',
      scope: {
        childcareEvent: '=',
        children: '='
      },
      templateUrl: 'childcareEvent/childcareEvent.html',
      controller: ChildcareEventController,
      controllerAs: 'childcareEvent',
      bindToController: true
    };

    function ChildcareEventController() {
      var vm = this;

    }
  }

})();
