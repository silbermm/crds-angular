'use strict()';

var app = require("angular").module("crossroads");
require("./volunteer_signup_form.html");
require('../profile/personal/profile_personal.html');
require('../profile');

app.controller("VolunteerController", require("./volunteer.controller"));
app.factory("Opportunity", ["$resource", "Session", require('./opportunity_service')]);
app.factory("ServeOpportunities", require('../services/serveOpportunities.service'));
