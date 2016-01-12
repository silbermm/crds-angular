(function() {
  'use strict';

  module.exports = AddEventComponent;

  AddEventComponent.$inject = [
    '$rootScope',
    'Lookup',
    'Programs',
    'StaffContact',
    'Validation'
  ];

  function AddEventComponent() {
    return {
      restrict: 'E',
      scope: {
        onNext: '&onNext',
        eventData: '='
      },
      templateUrl: 'add_event/add_event.html',
      controller: AddEventController,
      controllerAs: 'evt',
      bindToController: true
    };
  }

  function AddEventController($rootScope, Lookup, Programs, StaffContact, Validation) {
    var vm = this;

    vm.crossroadsLocations = Lookup.query({ table: 'crossroadslocations' });
    vm.endDateOpen = endDateOpen;
    vm.endDateOpened = false;
    vm.eventTypes = Lookup.query({ table: 'eventtypes' });
    vm.formatContact = formatContact;
    vm.next = next;
    vm.programs = Programs.AllPrograms.query();
    vm.reminderDays = Lookup.query({ table: 'reminderdays' });
    vm.staffContacts = StaffContact.query();
    vm.startDateOpen = startDateOpen;
    vm.startDateOpened = false;
    vm.validation = Validation;

    activate();

    ///////
    function activate() {
      if (vm.eventData !== undefined && Object.keys(vm.eventData).length > 0) {
        vm.formData = angular.copy(vm.eventData);
      } else {
        vm.formData = {
          donationBatch: 0,
          sendReminder: 0,
          minutesSetup: 0,
          minutesCleanup: 0
        };
      }
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
      if (vm.eventForm.$valid) {
        vm.onNext({data: vm.formData});
      } else {
        $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
      }
    }

    function startDateOpen($event) {
      $event.preventDefault();
      $event.stopPropagation();
      vm.startDateOpened = true;
    }

  }

})();
