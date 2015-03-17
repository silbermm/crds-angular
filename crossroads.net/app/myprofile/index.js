var app = require("angular").module("crossroads");
require('./myprofile.html');
app.controller("MyProfileCtrl", ['$scope', require("./myprofile_controller")]);