require('./content.html');
require('../templates/noHeaderOrFooter.html');
require('../templates/noSideBar.html');
require('../templates/rightSideBar.html');
require('../templates/leftSideBar.html');
require('../templates/screenWidth.html');

var app = angular.module("crossroads.core");
app.controller("ContentCtrl", ['$rootScope', '$scope', '$state', '$stateParams', '$log', 'ContentPageService', require("./content_controller")]);
