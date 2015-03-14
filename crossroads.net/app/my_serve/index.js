'use strict()';

var _ = require('lodash');

require('./myserve.html');
var app = require("angular").module('crossroads');

var MyServeController = require('./myserve.controller');
app.controller("MyServeController", MyServeController);
MyServeController.$inject = ['$log', 'ServeOpportunities'];

require('./serveTabs.html');
var ServeTabsDirective = require('./serveTabs.directive');
app.directive("serveTabs", ServeTabsDirective);
ServeTabsDirective.$inject = ['$log'];

app.factory("ServeOpportunities", require('../services/serveOpportunities.service'));
