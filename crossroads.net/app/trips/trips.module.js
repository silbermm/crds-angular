'use strict';
var MODULE = 'crossroads.trips';

angular.module(MODULE, ['crossroads.core', 'crossroads.give', 'crossroads.common'])
  .config(require('./trips.routes'))
  .factory('TripsUrlService', require('./tripsUrl.service'))
  .factory('Trip', require('./trips.service'))
  ;
require('./mytrips');
require('./tripsearch');
require('./tripgiving');
require('./signup');
