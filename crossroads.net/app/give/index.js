require('./give.html');
require('./amount.html');
require('./login.html');
require('./confirm.html');
require('./account.html');
require('./change.html');
require('./thank_you.html');
require('./register.html');
require('./history.html');
require('./give.module.js');
require('./give.config.js');

var app = angular.module('crossroads.give');

app.config(require('./give.routes'));

app.controller('GiveController', require('./give.controller'));
