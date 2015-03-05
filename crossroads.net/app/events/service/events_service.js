'use strict()';
﻿/**
 * A service for retrieving current events for a Crossroads site.
 */
(function(){
	/**
	 * Get an ngResource for the Gateway events API REST endpoint.
	 * @constructor
	 * @param {ngResource} $resource - The angular resource to instantiate
	 * @param {angular.$log} $log - The angular log service
	 * @returns {ngResource} an angular resource for the events API endpoint
	 */
	module.exports = function($resource, $log) {
		$log.debug("Inside Events factory");
		var eventsService = {
			res: $resource(__API_ENDPOINT__ + 'api/events/:site'),
			getDailyEvents: function(site) {
				var events = this.res.query({site:site});
				return(events);
			}
		};
		return(eventsService);
	};
})()
