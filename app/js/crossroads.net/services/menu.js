'use strict';

angular.module('crossroads')

.factory('Menu', function($window) {
  var Menu = (function() {
    function Menu(menuData) {
      this.menuData = menuData;
    }

    Menu.prototype.toggleMobileDisplay = function() {
      this.isMobileShowing = !this.isMobileShowing;
    };

    Menu.prototype.collapsed = function(index) {
      return index !== this.selectedMenuItem;
    };

    Menu.prototype.toggleMenuItem = function(index) {
      if (this.collapsed(index)) {
        this.selectedMenuItem = index;
      } else {
        this.selectedMenuItem = null;
      }
    };

    return Menu;

  })();
  return new Menu($window.menuData);
});
