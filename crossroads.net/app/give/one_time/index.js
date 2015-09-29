'use strict';

var constants = require('../../constants');

require('./templates/confirm.html');
require('./templates/account.html');
require('./templates/change.html');
require('./templates/thank_you.html');
require('./templates/login.html');

var app = angular.module(constants.MODULES.GIVE);

app.config(require('./one_time.routes'));
app.factory('OneTimeGiving', require('./one_time_giving.service'));
