'use strict()';

var _ = require('lodash');

require('./myserve.html');
var app = require("angular").module('crossroads');

app.controller("MyServeController", require('./myserve.controller'));

require('./serveTabs.html');
app.directive("serveTabs", require('./serveTabs.directive'));

app.factory("ServeOpportunities", require('../services/serveOpportunities.service'));
