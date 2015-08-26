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

app.factory('GiveTransferService', require('./services/giveTransfer.service.js'));

app.controller('GiveController', require('./give.controller'));
app.directive('naturalNumber', require('./directives/naturalNumber.validation.directive.js'));
app.directive('invalidRouting', require('./directives/invalidRouting.validation.directive.js'));
app.directive('invalidAccount', require('./directives/invalidAccount.validation.directive.js'));
app.directive('invalidZip', require('./directives/invalidZip.validation.directive.js'));
app.directive('initiativeRequired', require('./directives/initiativeRequired.validation.directive.js'));
