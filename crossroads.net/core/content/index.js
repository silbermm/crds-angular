require('./content.html');
var app = angular.module("crossroads.core");
app.controller("ContentCtrl", ['$rootScope', '$scope', '$state', '$stateParams', '$log', 'ContentPageService', require("./content_controller")]);
