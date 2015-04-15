'use strict';

require('./donationDetails.directive.js');
require('./donationConfirmation.directive');
require('./bankInfo.directive');

var app = require('angular');

app.module('give', ['donation-details', 'bank-info', 'donation-confirmation'])
