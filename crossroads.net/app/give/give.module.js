'use strict';
var stripe = require ('stripe');

require('angular-stripe');
angular.module('crossroads.give', ['angular-stripe','crossroads.core']);

require('./directives/donationDetails.directive');
require('./directives/donationConfirmation.directive');
require('./directives/bankInfo.directive');
require('./directives/creditCardInfo.directive');
require('./directives/currencyMask.directive');
require('./directives/bankCreditCardDetails.directive');
