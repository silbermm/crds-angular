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

  function AddEventController(Lookup, Programs, StaffContact) {
    var vm = this;

    vm.crossroadsLocations = Lookup.query({ table: 'crossroadslocations' });
    vm.endDateOpen = endDateOpen;
    vm.endDateOpened = false;
    vm.eventTypes = Lookup.query({ table: 'eventtypes' });
    vm.formatContact = formatContact;
    vm.formData = { };
    vm.next = next;
    vm.programs = Programs.AllPrograms.query();
    vm.reminderDays = Lookup.query({ table: 'reminderdays' });
    vm.staffContacts = StaffContact.query();
    vm.startDateOpen = startDateOpen;
    vm.startDateOpened = false;

    activate();

    ///////
    function activate() {
    
    }

    function endDateOpen($event) {
      $event.preventDefault();
      $event.stopPropagation();
      vm.endDateOpened = true;
    }
  
    function formatContact(contact) {
      var displayName = contact['Display Name'];
      var email = contact.dp_RecordName;
      return displayName + ' - ' + email;
    }

    function next() {
      // validate the form, then pass all the data back up
      // TODO: validate
      vm.onNext({data: {}});
    }

    function startDateOpen($event) {
      $event.preventDefault();
      $event.stopPropagation();
      vm.startDateOpened = true;
    }

  }

})();
