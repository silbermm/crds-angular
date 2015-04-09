'use strict';

var app =
require("angular").module('crossroads');
require('./go_trip_giving.html');
require('./go_trip_giving_results.html');
require('./go_trip_giving_search_form.html');

app.controller("GoTripGivingCtrl", ['$scope', require("./go_trip_giving_controller")]);

