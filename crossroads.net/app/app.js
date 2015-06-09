'use strict';

require('./profile');
require('./events');
require('./give');
require('./mp_tools');

"use strict";
(function () {

   angular.module("crossroads", [
     'crossroads.core',
     "crossroads.profile",
     'crossroads.mptools',
     'crossroads.give',
     'ngAside',
     'matchMedia'
     ])

    require('./app.controller');
    require('./app.run');
    require('./app.config');
    require('./routes');
})()
