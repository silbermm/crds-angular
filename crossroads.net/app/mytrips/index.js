'use strict';

var app =  angular.module('crossroads');
require('./mytrips.html');

app.controller("MyTripsCtrl", ['$scope', require("./mytrips_controller")]);