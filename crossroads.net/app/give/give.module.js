'use strict';

var constants = require('../constants');

require('./templates/give.html');
require('./templates/amount.html');
require('./templates/login.html');
require('./templates/register.html');

var app = angular.module(constants.MODULES.GIVE, [
    constants.MODULES.CORE,
    constants.MODULES.COMMON
])
.constant('GIVE_PROGRAM_TYPES', { Fuel: 1, Events: 2, Trips: 3, NonFinancial: 4 })
.constant('GIVE_ROLES', { StewardshipDonationProcessor: 7 })
;

app.config(require('./give.routes'));
require('./one_time');
app.controller('GiveController', require('./give.controller'));

