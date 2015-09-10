'use strict';
var MODULE = 'crossroads.trips';

angular.module(MODULE, ['crossroads.core', 'crossroads.give', 'crossroads.profile'])
  .config(require('./trips.routes'))
  .factory('TripsUrlService', require('./tripsUrl.service'));
require('./mytrips');
require('./tripsearch');
require('./tripgiving');
require('./signup');
