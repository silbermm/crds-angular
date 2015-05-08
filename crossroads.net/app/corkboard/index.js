'use strict';

var app = require("angular").module('crossroads');

require('./corkboard-listings.html');
require('./corkboard-create.html');
require('./forms/give-something.html');

app.controller("CorkboardCtrl", ['$scope', require("./corkboard_controller")]);
