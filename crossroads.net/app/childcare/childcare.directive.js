(function() {
  'use strict';

  module.exports = Childcare;

  Childcare.$inject = ['ChildcareEvents'];

  function Childcare(ChildcareEvents) {
    return {
      restrict: 'E',
      scope: { },
      templateUrl: 'childcare/childcare.html',
      controller: ChildcareController,
      controllerAs: 'childcare',
      bindToController: true
    };

    function ChildcareController() {
      var vm = this;

      // gets the route resolved event
      vm.childcareEvent = ChildcareEvents.childcareEvent;
      vm.children = ChildcareEvents.children;
      vm.event = ChildcareEvents.event;


      /////////////////////////

    }
  }

})();
