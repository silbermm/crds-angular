'use strict';

angular.module('crossroads')

.directive('crdsMenu', function() {
  return {
    controller: 'crdsMenuCtrl',
    templateUrl: 'crossroads.net/templates/crdsMenu.html',
    require: '?authForm',
    priority: 99,
    scope: {
      menus: '=crdsMenu',
      securityContext: '='
    }
  };
});
