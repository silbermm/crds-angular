var app = require("angular").module("crossroads");
require('./myprofile.html');
require('./templates/profile_personal.html');
require('./templates/profile_account.html');
require('./templates/profile_giving.html');
require('./templates/profile_history.html');
require('./templates/profile_household.html');
require('./templates/profile_skills.html');
app.controller("MyProfileCtrl", ['$scope', '$log', '$location', '$anchorScroll', require("./myprofile_controller")]);