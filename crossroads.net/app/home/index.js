
require('./home.html');
var app = angular.module("crossroads.core");

app.controller("HomeCtrl", ['$scope', require("./home_controller")]);
