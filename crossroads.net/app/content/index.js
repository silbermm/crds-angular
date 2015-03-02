var app = require("angular").module("crossroads");
app.controller("ContentCtrl", ['$scope', '$stateParams', '$log', 'Page', require("./content_controller")]);