(function() {
  'use strict';

  module.exports = ProfileController;
  ProfileController.$inject = [
    'AttributeTypes',
    'Person'];

  function ProfileController(AttributeTypes, Person) {
    var vm = this;

    vm.attributeTypes = AttributeTypes;
    vm.buttonText = 'Save';
    vm.profileData = { person: Person };
  }
})();
