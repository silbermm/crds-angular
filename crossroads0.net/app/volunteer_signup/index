'use strict()';

require('./view_opportunities.html');
var app = require("angular").module("crossroads");


app.controller("ViewOpportunitiesController", require("./view_opportunities_controller"));
app.factory("Opportunity", ["$resource", "Session", require('./opportunity_service')]);
app.factory("ServeOpportunities", require('../services/serveOpportunities.service'));
