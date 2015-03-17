'use strict()';

require('./view_opportunities.html');
var app = require("angular").module("crossroads");


app.controller("ViewOpportunitiesController", ["$log", "MESSAGES", "Opportunity", require("./view_opportunities_controller")]);
app.factory("Opportunity", ["$resource", "Session", require('./opportunity_service')]);
