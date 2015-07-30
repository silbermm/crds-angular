(function () {
  'use strict';
  module.exports = GoTripsCtrl;

  GoTripsCtrl.$inject = ['$scope', '$stateParams', '$log', '$location', '$anchorScroll'];

function GoTripsCtrl($scope, $stateParams, $log, $location, $anchorScroll) {
﻿
		var vm = this;

		vm.isCollapsed = true;
		vm.phoneToggle = true;
    vm.pageTitle = $stateParams.trip_location;
    vm.friendlyPageTitle;

    switch(vm.pageTitle) {
      case 'nola':
        vm.friendlyPageTitle = 'New Orleans';
        break;
      case 'south-africa':
        vm.friendlyPageTitle = 'South Africa';
        break;
      case 'india':
        vm.friendlyPageTitle = 'India';
        break;
    }

		vm.buttonClickBack = function(){
			console.log("in here");
		};

		vm.householdPhoneFocus = function () {
			vm.isCollapsed = false;

			$location.hash('homephonecont');

			setTimeout(function () {
					$anchorScroll();
			}, 500);

		};
	};
})()
