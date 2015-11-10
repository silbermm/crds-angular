(function() {
  'use strict';

  module.exports = ChildcareEvent;

  ChildcareEvent.$inject = ['$rootScope'];

  function ChildcareEvent($rootScope) {
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
      vm.form = {};
      vm.submit = submit;

      ////////////////

      function submit() {
        var childrenToSave = _.find(vm.children, function(child) {
          return child.selected;
        });

        if (_.isEmpty(childrenToSave)) {
          $rootScope.$emit('notify', $rootScope.MESSAGES.chooseOne);
          return false;
        } else {

          var childrenObj = _.map(childrenToSave, function(child) {
          });

          return true;
        }
      }
    }
  }

})();
