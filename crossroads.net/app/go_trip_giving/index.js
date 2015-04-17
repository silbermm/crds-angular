'use strict';

var app =
require("angular").module('crossroads');
require('./go_trip_giving.html');

app.controller("GoTripGivingCtrl", ['$scope', require("./go_trip_giving_controller")]);
