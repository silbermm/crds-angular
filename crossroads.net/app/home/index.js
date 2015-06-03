
require('./home.html');
var app = angular.module("crossroads");

app.controller("HomeCtrl", ['$scope', require("./home_controller")]);
