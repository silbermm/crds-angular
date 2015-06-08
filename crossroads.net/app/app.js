'use strict';

require('./profile');
require('./filters');
require('./events');

require('./give');

require('./components/components.module');
require('./mp_tools');

"use strict";
(function () {

   angular.module("crossroads", [
     'crossroads.core',
     "crossroads.profile",
     "crossroads.filters",
     'crossroads.mptools',
     'crossroads.components',
     'crossroads.give',
     'ngAside',
     'matchMedia'
     ])

    require('./app.controller');
    require('./app.run');
    require('./app.config');
    require('./routes');
})()
