(function() {
  'use strict';

  module.exports = ProfileController;
  ProfileController.$inject = [
    'AttributeTypes',
    'Profile'];

  function ProfileController(AttributeTypes, Profile) {
    var vm = this;

    vm.attributeTypes = AttributeTypes;
    vm.buttonText = 'Save';
    //TODO Move to resolve
    Profile.Personal.get(function(data) {
      vm.profileData = { person: data };
      vm.viewReady = true;
    });
  }
})();
