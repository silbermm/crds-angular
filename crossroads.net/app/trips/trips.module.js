'use strict';
var MODULE = 'crossroads.trips';

require('../give');

angular.module(MODULE, ['crossroads.core', 'crossroads.give'])
  .config(require('./trips.routes'))
  .factory('TripsUrlService', require('./tripsUrl.service'));
require('./mytrips');
require('./tripsearch');
require('./tripgiving');
require('./signup');
