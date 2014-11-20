'use strict';

angular.module('crossroads')

.controller('MainCtrl', function(Menu) {

  this.menus = Menu.menuData;

  this.toggleMenu = function() {
    Menu.toggleMobileDisplay();
  };
});
