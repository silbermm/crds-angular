(function() {
  'use strict';

  module.exports = AddEventComponent;

  AddEventComponent.$inject = [];

  function AddEventComponent() {
    return {
      restrict: 'E',
      scope: {
        onNext: '&onNext'
      },
      templateUrl: 'add_event/add_event.html',
      controller: AddEventController,
      controllerAs: 'evt',
      bindToController: true
    };
  }

  function AddEventController() {
    var vm = this;

    vm.next = next;

    ///////
    function next() {
      // validate the form, then pass all the data back up
      // TODO: validate
      vm.onNext({data: {}});
    }

  }

})();
