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
    vm.locations = getLocations();
    vm.attributeTypes = AttributeTypes;
    vm.buttonText = 'Save';
    vm.profileData = { person: Person };
    vm.locationFocus = locationFocus;
    vm.displayLocation = displayLocation;

    function locationFocus() {
      $rootScope.$emit('locationFocus');
    }

    function getLocations() {
      return Lookup.query({
        table: 'crossroadslocations'
      }, function(data) {
        return data;
      });
    }

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
  }
})();
