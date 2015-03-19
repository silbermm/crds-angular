var app = require("angular").module("crossroads");
require('./give.html');
app.controller("GiveCtrl", ['$scope', require("./give_controller")]);