/**
 * A service for retrieving events for Crossroads.
 */
(function() {
  'use strict';
  /**
   * Get an ngResource for the Gateway events API REST endpoint.
   * @constructor
   * @param {ngResource} $resource - The angular resource to instantiate
   * @param {angular.$log} $log - The angular log service
   * @returns {ngResource} an angular resource for the events API endpoint
   */
  module.exports = EventService;

  EventService.$inject = ['$resource', '$log'];

  function EventService($resource, $log) {
    var eventsService = {
      res: $resource(__API_ENDPOINT__ + 'api/events/:site'),
      event: $resource(__API_ENDPOINT__ + 'api/event/:eventId'),
      getDailyEvents: function(site) {
        var events = this.res.query({site:site});
        return events;
      }
    };
    return eventsService;
  }
})();
