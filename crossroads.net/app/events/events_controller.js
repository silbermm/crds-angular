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
			$log.debug("Received response from events API for site " + $location.search().site);
			$scope.data.events = response;
			$log.debug("Events returned from service: " + JSON.stringify($scope.data.events));
		});
	};
})()
