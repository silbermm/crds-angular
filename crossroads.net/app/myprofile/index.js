var app = require("angular").module("crossroads");
require('./myprofile.html');
app.controller("MyProfileCtrl", ['$scope', '$log', '$location', '$anchorScroll', require("./myprofile_controller")]);