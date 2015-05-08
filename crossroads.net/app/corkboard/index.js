'use strict';

var app =
  require("angular").module('crossroads');
require('./corkboard-listings.html');
require('./corkboard-listing-detail.html');

app.controller("CorkboardCtrl", ['$scope', require("./corkboard_controller")]);