var app = angular.module("crossroads");
require('./explore.html');
require('./scripts.html');

app.controller("ExploreCtrl", require("./explore_controller"));
