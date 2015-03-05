'use strict';
﻿/**
 * A controller for retrieving current events for a Crossroads site.
 */
(function() {
	module.exports =  function ($scope, $log, $http, $location, Events){
		$log.debug("EventsController loaded");
		// Initialize data object, will get populated with response
		$scope.data = {};
		Events.query({site:$location.search().site}, function(response) {
			$scope.data.events = response;
		});
		$log.debug("Events returned from service: " + $scope.data);
	};
})()
