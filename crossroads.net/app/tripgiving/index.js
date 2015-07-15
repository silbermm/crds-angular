'use strict';

var app = angular.module('crossroads');
require('./tripgiving.html');

app.controller("TripGivingCtrl", ['$scope', require("./tripgiving.controller")]);
