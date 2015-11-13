(function() {
  'use strict';

  module.exports = ProfileController;

  ProfileController.$inject = [
    '$rootScope',
    '$state',
    '$location',
    '$window',
    'AttributeTypes',
    'Person',
    'Profile',
    'Lookup',
    'Locations',
    'PaymentService'];

  function ProfileController(
      $rootScope,
      $state,
      $location,
      $window,
      AttributeTypes,
      Person,
      Profile,
      Lookup,
      Locations,
      PaymentService) {

    var vm = this;
    vm.attributeTypes = AttributeTypes;
    vm.buttonText = 'Save';
    vm.displayLocation = displayLocation;
    vm.enforceAgeRestriction = enforceAgeRestriction;
    vm.goToTab = goToTab;
    vm.locations = Locations;
    vm.locationFocus = locationFocus;
    vm.profileData = { person: Person };
    vm.tabs = [
      { title:'Personal', active: false, route: 'profile.personal' },
      { title:'Communications', active: false, route: 'profile.communications' },
      { title:'Skills', active: false, route: 'profile.skills' },
      { title: 'Giving History', active: false, route: 'profile.giving' }
    ];

    $rootScope.$on('$stateChangeStart', stateChangeStart);
    $window.onbeforeunload = onBeforeUnload;

    activate();

    ////////////
    function activate() {
      _.forEach(vm.tabs, function(tab) {
        tab.active = $state.current.name === tab.route;
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

    function enforceAgeRestriction() {
      return 13;
    }

    function locationFocus() {
      $rootScope.$emit('locationFocus');
    }


    function goToTab(tab) {
      $state.go(tab.route);
    }

    function stateChangeStart(event, toState, toParams, fromState, fromParams) {
      if (fromState.name === 'profile.personal' && vm.profileParentForm.$dirty) {
        if (!$window.confirm('Are you sure you want to leave this page?')) {
          event.preventDefault();
          vm.tabs[0].active = true;
          return;
        }
      }
    }

    function onBeforeUnload() {
      if (vm.profileParentForm.$dirty) {
        return '';
      }
    }
  }
})();
