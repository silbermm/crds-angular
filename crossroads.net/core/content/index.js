require('./content.html');
require('./sidebarContent.html');
require('../templates/noHeaderOrFooter.html');
require('../templates/noSideBar.html');
require('../templates/rightSideBar.html');
require('../templates/leftSidebar.html');
require('../templates/screenWidth.html');
require('../templates/fullWidth.html');

var app = angular.module('crossroads.core');
app.controller('ContentCtrl',
  ['$rootScope',
   '$scope',
   '$state',
   '$stateParams',
   '$log',
   'ContentPageService',
   require('./content_controller')]);
