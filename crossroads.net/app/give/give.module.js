'use strict';

require('./directives/donationDetails.directive.js');
require('./directives/donationConfirmation.directive');
require('./directives/bankInfo.directive');
require('./directives/creditCardInfo.directive');
require('./directives/currencyMask.directive');

var app = require('angular');

app.module('give', ['donation-details', 'bank-info', 'donation-confirmation', 'credit-card-info', 'currencyMask'])
