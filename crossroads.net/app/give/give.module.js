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
]);

app.config(require('./give.routes'));
app.controller('GiveController', require('./give.controller'));
app.factory('OneTimeGiving', require('./oneTimeGiving.service'));
