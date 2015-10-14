(function() {
  'use strict';

  module.exports = ProfileController;
  ProfileController.$inject = [
    'contactId',
    'Person'];

  function ProfileController(contactId, Person) {
    var vm = this;

    vm.contactId = contactId;
    vm.buttonText = 'Save';
    vm.profileData = { person:  Person };
  }
})()
