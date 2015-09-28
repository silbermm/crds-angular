'use strict';

var constants = require('../../constants');

require('./templates/account.html');
require('./templates/thank_you.html');

var app = angular.module(constants.MODULES.GIVE);

app.config(require('./recurring.routes'));
app.factory('RecurringGiving', require('./recurring_giving.service'));
