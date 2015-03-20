
require('./home.html');
var app = require("angular").module("crossroads");

app.controller("HomeCtrl", ['$scope', require("./home_controller")]);
