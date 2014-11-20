'use strict';

angular.module('crossroads')

.controller('LoginCtrl', function($scope, SecurityContext) {

  $scope.user = {};

  $scope.login = function() {
    var promise = SecurityContext.login($scope.user.username,
                                        $scope.user.password);

    if (this.user.username && this.user.password) {
      $scope.processing = 'Logging in...';

      promise.then(function() {
        $scope.loginError = void 0;
      }, function() {
        $scope.processing = null;
        $scope.loginError = ['Oops!', 'Login failed.'];
      });
    } else {
      $scope.loginError = ['Hold up!', 'Username/password can\'t be blank'];
    }
  };
});
