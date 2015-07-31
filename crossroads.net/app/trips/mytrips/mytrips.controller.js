(function () {
  	'use strict';
  	module.exports = MyTripsController;
  	MyTripsController.$inject=['$log', 'MyTrips'];

  	function MyTripsController($log, MyTrips) {
  		var vm = this;
  		vm.myTrips = MyTrips.myTrips;
	}
})()