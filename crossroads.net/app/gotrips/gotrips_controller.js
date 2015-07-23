'use strict';
﻿/**
 * A controller for retrieving go trips for a Crossroads site.
 */
(function() {
	module.exports =  function ($scope, $log, $location, $anchorScroll) {
		var vm = this;

		vm.isCollapsed = true;
		vm.phoneToggle = true;

		vm.householdPhoneFocus = function () {
			vm.isCollapsed = false;

			$location.hash('homephonecont');
			
			setTimeout(function () {
					$anchorScroll();
			}, 500);

		};
	};
})()
