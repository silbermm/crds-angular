'use strict';
(function() {
  module.exports = ProfileHouseholdController;

  ProfileHouseholdController.$inject = ['$rootScope', '$location', '$anchorScroll', '$log', 'Profile', 'Lookup'];

  function ProfileHouseholdController($rootScope, $location, $anchorScroll, $log, Profile, Lookup) {
    var vm = this;

    vm.countries = getCountries();
    vm.displayName = displayName;
    vm.displayLocation = displayLocation;
    vm.isCollapsed = true;
    vm.locations = getLocations();
    vm.states = getStates();

    $log.debug('householdId: ' + vm.householdId);

    if (vm.householdId) {
      Profile.Household.get({ householdId: vm.householdId}, function(data) {
        vm.info = data;
      });
    }

    $rootScope.$on('homePhoneFocus', function(event,data) { 
      vm.isCollapsed = false;

      $location.hash('homephonecont');

      setTimeout(function () {
        $anchorScroll();
      }, 500);
    });


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
