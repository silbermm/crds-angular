'use strict()';

require("./volunteer_signup_form.html");
var app = require("angular").module("crossroads");


app.controller("VolunteerController", require("./volunteer.controller"));
app.factory("Opportunity", ["$resource", "Session", require('../opportunity/opportunity_service')]);
app.factory("ServeOpportunities", require('../services/serveOpportunities.service'));

// Participants: function(ServeOpportunities){
//   return ServeOpportunities.QualifiedServers.query({groupId: 27705, contactId: getCookie('userId')}).$promise;
// }
