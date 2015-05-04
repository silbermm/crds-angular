'use strict';
var app = require('angular');
var stripe = require ('stripe');

require('angular-stripe');

app.module('crossroads.give', ['angular-stripe']);

require('./directives/donationDetails.directive.js');
require('./directives/donationConfirmation.directive');
require('./directives/bankInfo.directive');
require('./directives/creditCardInfo.directive');
require('./directives/currencyMask.directive');
