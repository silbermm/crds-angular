require('crds-core');
require('../../../app/trips/trips.module');

(function() {
  'use strict';

  describe('Trip Donate Card Directive', function() {

    beforeEach(angular.mock.module('crossroads.trips'));

    beforeEach(angular.mock.module(function($provide) {
      $provide.value('MyTrips', myTrips);
    }));

  });
})();
