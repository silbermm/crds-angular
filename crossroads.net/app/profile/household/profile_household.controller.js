'use strict';
(function() {
  module.exports = ProfileHouseholdController;

  ProfileHouseholdController.$inject = ['$log', 'Profile', 'Lookup'];

  function ProfileHouseholdController($log, Profile, Lookup) {
    var vm = this;

    vm.displayName = displayName;
    vm.displayLocation = displayLocation;
    vm.countries = getCountries();
    vm.locations = getLocations();
    vm.states = getStates();

    $log.debug('householdId: ' + vm.householdId);

    if (vm.householdId) {
      Profile.Household.get({ householdId: vm.householdId}, function(data) {
        vm.info = data;
      });
    }

    function displayName(member) {
      return member.nickname + ' ' + member.lastName;
    }

    function displayLocation(locationId) {
      return _.result(_.find(vm.locations, 'dp_RecordID', locationId), 'dp_RecordName');
    }

    function getCountries() {
      return Lookup.query({
        table: 'countries'
      }, function(data) {
        return data;
      });
    }

    function getLocations() {
      return Lookup.query({
        table: 'crossroadslocations'
      }, function(data) {
        return data;
      });
    }

    function getStates() {
      return Lookup.query({
        table: 'states'
      }, function(data) {
        return data;
      });
    }
  }
})();
