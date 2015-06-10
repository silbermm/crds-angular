'use strict()';

var app = require("angular").module("crossroads");
require("./volunteerApplicationForm.html");
require('../profile/personal/profile_personal.html');
require('../profile');



//constant isn't working, figure out why??????????????
// require('./studentConstants');
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
  "howLongAttending": 314,
  "serviceAttend": 474,
  "streetAddress": 319,
  "city": 320,
  "state": 321,
  "zip": 322,
  "mobilePhone": 323,
  "homePhone": 324,
  "school": 326,
  "grade": 327,
  "whereYouAre": 330,
  "explainFaith": 331,
  "whyServe": 332,
  "specialTalents": 333,
  "availabilityDuringWeek": 484,
  "availabilityDuringWeekend": 485,
  "serveSite": 338,
  "availabilityOakley": 476,
  "availabilityFlorence": 477,
  "availabilityWestSide": 478,
  "availabilityMason": 479,
  "availabilityClifton": 480,
  "serveServiceTimes": 340,
  "serveAgeKids1to2": 481,
  "serveAgeKids3toPreK": 482,
  "serveAgeKidsKto5Grade": 483,
  "reference1Name": 346,
  "reference1timeKnown": 347,
  "reference1homePhone": 349,
  "reference1mobilePhone": 350,
  "reference1workPhone": 351,
  "reference1email": 352,
  "reference1association": 353,
  "reference1occupation": 354,
  "reference2Name": 355,
  "reference2timeKnown": 356,
  "reference2homePhone": 358,
  "reference2mobilePhone": 359,
  "reference2workPhone": 360,
  "reference2email": 361,
  "reference2association": 362,
  "reference2occupation": 363,
  "parentLastName": 366,
  "parentFirstName": 367,
  "parentHomePhone": 368,
  "parentMobilePhone": 369,
  "parentEmail": 370,
  "parentSignature": 371,
  "parentSignatureDate": 372,
  "studentSignature": 373,
  "studentSignatureDate": 374
});
