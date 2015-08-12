'use strict'; 
require('crds-core');
var MODULE = 'crossroads.trips';

angular.module(MODULE, ['crossroads.core']).config(require('./trips.routes'));

require('./mytrips');
require('./tripgiving');
