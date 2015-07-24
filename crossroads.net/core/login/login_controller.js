(function () {
  'use strict';
  module.exports = function LoginController(
    $scope, 
    $rootScope, 
    AUTH_EVENTS, 
    MESSAGES, 
    AuthService, 
    $state, 
    $log, 
    Session, 
    $timeout, 
    User) {

    $log.debug('Inside Login controller');
    $scope.loginShow = false;
    $scope.newuser = User;
    $scope.credentials = {};
    $scope.credentials.username = $scope.newuser.email;
    $scope.passwordPrefix = 'login-page';
    $scope.checkEmail = function() {
        return ($scope.navlogin.username.$error.required && $scope.navlogin.$submitted && $scope.navlogin.username.$dirty ||
            $scope.navlogin.username.$error.required && $scope.navlogin.$submitted && !$scope.navlogin.username.$touched ||
            $scope.navlogin.username.$error.required && $scope.navlogin.$submitted && $scope.navlogin.username.$touched || 
            !$scope.navlogin.username.$error.required && $scope.navlogin.username.$dirty && !$scope.navlogin.username.$valid);
    };

    $scope.toggleDesktopLogin = function () {
        $scope.loginShow = !$scope.loginShow;
        if ($scope.registerShow) {
            $scope.registerShow = !$scope.registerShow;
            $scope.credentials.username = $scope.newuser.email;
            $scope.credentials.password = $scope.newuser.password;
        }
    };

    $scope.logout = function() {
        $state.go('logout');
        return;
    };

    $scope.login = function () {
        if (($scope.credentials === undefined) || 
            ($scope.credentials.username === undefined || 
            $scope.credentials.password === undefined)) {
            $scope.pending = true;
            $scope.loginFailed = false;
            $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);

        } else {
            $scope.processing = true;
            AuthService.login($scope.credentials).then(function (user) {
                $scope.processing = false;
                $scope.loginShow = false;
                clearCredentials();
                $timeout(function() {
                    if (Session.hasRedirectionInfo()) {
                        var url = Session.exists('redirectUrl');
                        var params = Session.exists('params');
                        Session.removeRedirectRoute();
                        if(params === undefined){
                            $state.go(url);
                        } else {
                            $state.go(url, JSON.parse(params));
                        }
                    }
                }, 500);
                $scope.loginFailed = false;
                $rootScope.showLoginButton = false;
                $scope.navlogin.$setPristine();
            }, function () {
                $scope.pending = false;
                $scope.processing = false;
                $scope.loginFailed = true;
                $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
            });
        }
    };
  

    function clearCredentials() {
        if ($scope.credentials !== undefined) {
            // TODO Added to debug/research US1403 - should remove after issue is resolved
            console.log('US1403: clearing credentials defined in login_controller');
            $scope.credentials.username = undefined;
            $scope.credentials.password = undefined;
        }
    }
};
})();
