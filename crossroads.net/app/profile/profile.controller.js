(function() {
  'use strict';

  module.exports = ProfileController;
  ProfileController.$inject = [
    'contactId',
    'Person',
    'AttributeTypes'];

  function ProfileController(contactId, Person, AttributeTypes) {
    var vm = this;

    vm.attributeTypes = AttributeTypes;
    vm.buttonText = 'Save';
    vm.contactId = contactId;
    vm.profileData = { person:  Person };
  }
})()
