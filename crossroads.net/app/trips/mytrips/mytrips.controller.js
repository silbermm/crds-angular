(function () {
  	'use strict';
  	module.exports = MyTripsController;
  	MyTripsController.$inject=['$rootScope', '$log', 'MyTrips'];

  	function MyTripsController($rootScope, $log, MyTrips) {
  		var vm = this;
  		vm.myTrips = MyTrips.myTrips;
	}
})()