'use strict';

angular.module('crossroads')

.directive('hideOnMobileMenu', function(Menu) {
  return {
    link: function(scope, element) {
      scope.menu = Menu;

      scope.$watch('menu.isMobileShowing', function() {
        if (scope.menu.isMobileShowing) {
          element.addClass('show');
        } else {
          element.removeClass('show');
        }
      });
    }
  };
});
