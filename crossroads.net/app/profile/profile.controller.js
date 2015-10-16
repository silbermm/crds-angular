(function() {
  'use strict';

  module.exports = ProfileController;
  ProfileController.$inject = [
    'contactId',
    'Person',
    'AttributeTypes',
    'Profile'];

  function ProfileController(contactId, Person, AttributeTypes, Profile) {
    var vm = this;

    vm.attributeTypes = AttributeTypes;
    vm.buttonText = 'Save';
    vm.contactId = contactId;
    Profile.Personal.get(function(data) {
      vm.profileData = { person: data };
      vm.viewReady = true;
      debugger;
    });
  }
})();
