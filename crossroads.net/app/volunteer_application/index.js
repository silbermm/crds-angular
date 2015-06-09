'use strict()';

var app = require("angular").module("crossroads");
require("./volunteerApplicationForm.html");
require('../profile/personal/profile_personal.html');
require('../profile');

app.controller("VolunteerApplicationController", require("./volunteerApplication.controller"));
app.factory("Opportunity", ["$resource", "Session", require('../services/opportunity_service')]);

require('./kc_adult_application/kidsClubAdultApplication.template.html');
app.directive("kidsClubAdultApplication", require('./kc_adult_application/kidsClubAdultApplication.directive'));

require('./kc_student_application/kidsClubStudentApplication.template.html');
app.controller("KidsClubStudentApplicationController", require('./kc_student_application/kidsClubStudentApplication.controller'));
app.directive("kidsClubStudentApplication", require('./kc_student_application/kidsClubStudentApplication.directive'));
app.factory("VolunteerService", ["$resource", require('./VolunteerService')]);

app.constant("studentFields", {
  "firstName": 310,
  "lastName": 311,
  "middleInitial": 312,
  "email": 325,
  "nameForNameTag": 318,
  "birthDate": 317,
  "gender": 315,
  "site": 313,
  "school": 326
});
