(function() {
  'use strict';

  module.exports = AddEventComponent;

  AddEventComponent.$inject = [
    '$rootScope',
    'AddEvent',
    'Lookup',
    'Programs',
    'StaffContact',
    'Validation'
  ];

  function AddEventComponent() {
    return {
      restrict: 'E',
      scope: {
        eventData: '='
      },
      templateUrl: 'add_event/add_event.html',
      controller: AddEventController,
      controllerAs: 'evt',
      bindToController: true
    };
  }

  function AddEventController($rootScope, AddEvent, Lookup, Programs, StaffContact, Validation) {
    var vm = this;

    vm.crossroadsLocations = Lookup.query({ table: 'crossroadslocations' });
    vm.endDateOpen = endDateOpen;
    vm.endDateOpened = false;
    vm.eventTypes = Lookup.query({ table: 'eventtypes' });
    vm.formatContact = formatContact;
    vm.programs = Programs.AllPrograms.query();
    vm.reminderDays = Lookup.query({ table: 'reminderdays' });
    vm.staffContacts = StaffContact.query();
    vm.startDateOpen = startDateOpen;
    vm.startDateOpened = false;
    vm.validation = Validation;

    activate();

    ///////
    function activate() {
      if (_.isEmpty(vm.eventData)) {
        vm.eventData = {
          donationBatch: 0,
          sendReminder: 0,
          minutesSetup: 0,
          minutesCleanup: 0,
          startTime: new Date(),
          endTime: new Date()
        };
      }
    }

    function endDateOpen($event) {
      $event.preventDefault();
      $event.stopPropagation();
      vm.endDateOpened = true;
    }

    function formatContact(contact) {
      var displayName = contact.displayName;
      var email = contact.email;
      return displayName + ' - ' + email;
    }

    function startDateOpen($event) {
      $event.preventDefault();
      $event.stopPropagation();
      vm.startDateOpened = true;
    }

  }

})();
