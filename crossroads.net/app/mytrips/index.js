'use strict';

var app =
require("angular").module('crossroads');
require('./mytrips.html');

app.controller("MyTripsCtrl", ['$scope', require("./mytrips_controller")]);