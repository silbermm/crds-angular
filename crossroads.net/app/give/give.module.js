'use strict';

require('./donationDetails.directive.js');
require('./donationConfirmation_directive');
require('./bankInfo_directive');

var app = require('angular');

app.module('give', ['donation-details', 'bank-info', 'donation-confirmation'])
