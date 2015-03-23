'use strict()';

var _ = require('lodash');

require('./myserve.html');
var app = require("angular").module('crossroads');

app.controller("MyServeController", require('./myserve.controller'));

require('./serveTabs.html');
app.directive("serveTabs", require('./serveTabs.directive'));

require('./serveTeam.html');
app.directive("serveTeam", require('./serveTeam.directive'));

app.factory("ServeOpportunities", require('../services/serveOpportunities.service'));
