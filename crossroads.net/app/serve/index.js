// var app = require("angular").module("crossroads");
// require('./give.html');
// app.controller("GiveCtrl", [ require("./give_controller")]);

'use strict()';

var app = require("angular").module("crossroads");
app.controller("ServeController", ["$log", "MESSAGES", require("./serve_controller")]);
//app.factory("Opportunity", ["$resource", "Session", require('./opportunity_service')]);
