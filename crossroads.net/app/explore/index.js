var app = angular.module("crossroads");
require('./explore.html');
require('./exploresnap.html');
require('./exploreNext');
require('./keyboardKeys');

app.controller("ExploreCtrl", require("./explore_controller"));
