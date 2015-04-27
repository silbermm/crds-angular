'use strict';
var app = require('angular');

app.module('crossroads.give', [])

require('./directives/donationDetails.directive.js');
require('./directives/donationConfirmation.directive');
require('./directives/bankInfo.directive');
require('./directives/creditCardInfo.directive');
require('./directives/currencyMask.directive');


