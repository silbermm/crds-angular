(function() {
  'use strict';

  module.exports = AddEventComponent;

  AddEventComponent.$inject = ['Lookup', 'Programs'];

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

  function AddEventController(Lookup, Programs) {
    var vm = this;

    vm.crossroadsLocations = Lookup.query({ table: 'crossroadslocations' });
    vm.eventTypes = Lookup.query({ table: 'eventtypes' });
    vm.next = next;
    vm.programs = Programs.Programs.query();
    vm.reminderDays = Lookup.query({ table: 'reminderdays' });

    activate();

    ///////
    function activate() {
    
    }
    
    function next() {
      // validate the form, then pass all the data back up
      // TODO: validate
      vm.onNext({data: {}});
    }

  }

})();
