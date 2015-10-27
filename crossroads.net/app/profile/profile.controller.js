(function() {
  'use strict';

  module.exports = ProfileController;

  ProfileController.$inject = [
    '$rootScope',
    'AttributeTypes',
    'Person',
    'Lookup'];

  function ProfileController($rootScope, AttributeTypes, Person, Lookup) {

    var vm = this;
    vm.attributeTypes = AttributeTypes;
    vm.buttonText = 'Save';
    vm.displayLocation = displayLocation;
    vm.enforceAgeRestriction = enforceAgeRestriction;
    vm.locations = getLocations();
    vm.locationFocus = locationFocus;
    vm.profileData = { person: Person };

    ////////////

    function displayLocation() {
      var locationName;
      if (vm.profileData.person.congregationId !== 2) {
        locationName = _.result(
          _.find(
            vm.locations,
            'dp_RecordID',
            vm.profileData.person.congregationId),
            'dp_RecordName');
      }

      if (!locationName) {
        return 'Select Crossroads Site...';
      }

      return locationName;
    }

    function enforceAgeRestriction() {
      return 13;
    }

    function getLocations() {
      return Lookup.query({
        table: 'crossroadslocations'
      }, function(data) {
        return data;
      });
    }

    function locationFocus() {
      $rootScope.$emit('locationFocus');
    }
  }
})();
