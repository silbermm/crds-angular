(function() {

  'use strict';
  module.exports = LogoutController;

  LogoutController.$inject = ['$rootScope', '$scope', '$log', 'AuthService', '$state'];

  function LogoutController($rootScope, $scope, $log, AuthService, $state) {
    $log.debug('Inside Logout-Controller');

    logout();

    function logout(){
      console.log('US1403: logging out user in logout_controller');
        AuthService.logout();
        $state.go('home');
    }
  }
})();
