'use strict';

angular.module('crossroads')

.controller('crdsMenuCtrl',
    function($scope, $rootScope, Menu, SecurityContext) {

  $scope.menu = Menu;
  $scope.loginShow = false;

  $scope.logout = function() {
    SecurityContext.logout();
  };

  $scope.toggleDesktopLogin = function() {
    if (this.loginShow === true) {
      this.loginShow = false;
    } else {
      this.loginShow = true;
    }
  };

  $scope.toggleMenuDisplay = function() {
    this.menu.toggleMobileDisplay();
  };

  $rootScope.$on('login:hide', function() {
    $scope.loginShow = false;
  });
});
