'use strict';

var constants = require('../constants');

require('./giveTemplates/give.html');
require('./giveTemplates/amount.html');
require('./giveTemplates/login.html');
require('./giveTemplates/confirm.html');
require('./giveTemplates/account.html');
require('./giveTemplates/change.html');
require('./giveTemplates/thank_you.html');
require('./giveTemplates/register.html');
require('./giveTemplates/history.html');

var app = angular.module(constants.MODULES.GIVE, [
    constants.MODULES.CORE,
    constants.MODULES.COMMON
])
.constant('GIVE_PROGRAM_TYPES', { Fuel: 1, Events: 2, Trips: 3, NonFinancial: 4 })
.constant('GIVE_ROLES', { StewardshipDonationProcessor: 7 })
;

app.config(require('./give.routes'));
app.controller('GiveController', require('./give.controller'));
app.factory('OneTimeGiving', require('./oneTimeGiving.service'));
