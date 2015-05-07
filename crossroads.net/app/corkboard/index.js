'use strict';

var app =
  require("angular").module('crossroads');
require('./corkboard-listings.html');

app.controller("CorkboardCtrl", ['$scope', require("./corkboard_controller")]);
