'use strict';

require('./donationDetails.directive.js');

var app = require('angular');

app.module('give', ['donation-details'])
