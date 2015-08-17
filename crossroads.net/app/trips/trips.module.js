'use strict'; 
var MODULE = 'crossroads.trips';

require('../give');

angular.module(MODULE, ['crossroads.core', 'crossroads.give'])
  .config(require('./trips.routes'));

require('./mytrips');
require('./tripsearch');
require('./tripgiving');
