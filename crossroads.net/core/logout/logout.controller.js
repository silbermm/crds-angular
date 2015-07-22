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
        if ($scope.credentials !== undefined) {
            // TODO Added to debug/research US1403 - should remove after issue is resolved
            console.log('US1403: clearing credentials defined in login_controller');
            $scope.credentials.username = undefined;
            $scope.credentials.password = undefined;
        }
        $rootScope.username = undefined;

        // if ($state.current === undefined || $state.current.data === undefined || !$state.current.data.isProtected) {
        //     // not currently on a protected page, so don't redirect to home page
        //     $event.preventDefault();
        // }
    }
  }
})();
