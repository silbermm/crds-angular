var app = require("angular").module("crossroads");
app.controller("ContentCtrl", ['$rootScope', '$scope', '$state', '$stateParams', '$log', 'Page', require("./content_controller")]);