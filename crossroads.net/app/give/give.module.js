'use strict';

var constants = require('../constants');

require('./give.html');
require('./amount.html');
require('./login.html');
require('./confirm.html');
require('./account.html');
require('./change.html');
require('./thank_you.html');
require('./register.html');
require('./history.html');

var app = angular.module(constants.MODULES.GIVE, [
    constants.MODULES.CORE,
    constants.MODULES.COMMON
]);

app.config(require('./give.routes'));
app.controller('GiveController', require('./give.controller'));
