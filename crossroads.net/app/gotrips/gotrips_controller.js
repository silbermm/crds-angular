(function () {
  'use strict';
  module.exports = GoTripsCtrl;

  GoTripsCtrl.$inject = ['$scope', '$log', '$location', '$anchorScroll'];

function GoTripsCtrl($scope, $log, $location, $anchorScroll) {
﻿
		var vm = this;

		vm.isCollapsed = true;
		vm.phoneToggle = true;

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
