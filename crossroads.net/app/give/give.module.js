'use strict';
//var stripe = require ('stripe');
require('angular-stripe');
angular.module('crossroads.give', ['angular-stripe','crossroads.core']);

require('./donation_details');
require('./directives/bankInfo.directive');
require('./directives/creditCardInfo.directive');
require('./directives/currencyMask.directive');
require('./directives/bankCreditCardDetails.directive');
