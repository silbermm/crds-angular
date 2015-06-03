'use strict';
var stripe = require ('stripe');

require('angular-stripe');
require('../app.core.module')
angular.module('crossroads.give', ['angular-stripe','crossroads.core']);

require('./directives/donationDetails.directive.js');
require('./directives/donationConfirmation.directive');
require('./directives/bankInfo.directive');
require('./directives/creditCardInfo.directive');
require('./directives/currencyMask.directive');
