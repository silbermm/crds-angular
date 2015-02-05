'use strict';
(function () {
  

    function LoginController($scope, $rootScope, AUTH_EVENTS, MESSAGES, AuthService, $cookieStore, $state, $log, Session, $timeout) {
  
        $scope.loginShow = false;

        $scope.toggleDesktopLogin = function () {
            $scope.loginShow = !$scope.loginShow;
            if ($scope.registerShow)
                $scope.registerShow = !$scope.registerShow;
        }
          
        $scope.logout = function () {
            AuthService.logout();
            if ($scope.credentials !== undefined) {
                $scope.credentials.username = undefined;
                $scope.credentials.password = undefined;
            }
            $rootScope.username = null;
        }

        $scope.login = function () {           
            if (($scope.credentials === undefined) || ($scope.credentials.username === undefined || $scope.credentials.password === undefined)) {
                $scope.pending = true;
                $scope.loginFailed = false;
            } else {
                $scope.processing = true;
                AuthService.login($scope.credentials).then(function (user) {             
                    $scope.processing = false;
                    $scope.loginShow = false;
                    $timeout(function() {
                        if (Session.hasRedirectionInfo()) {
                            var url = Session.exists("redirectUrl");
                            var params = Session.exists("redirectParams");
                            Session.removeRedirectRoute();
                            $state.go(url);
                        }
                    }, 500);
                    $scope.loginFailed = false;
                    $rootScope.showLoginButton = false;
                    $scope.navlogin.$setPristine();
                }, function () {
                    $log.debug("Bad password");
                    $scope.pending = false;
                    $scope.processing = false;
                    $scope.loginFailed = true;
                });
            }
        };
    }

    angular.module('crossroads').controller('LoginCtrl', ['$scope', '$rootScope', 'AUTH_EVENTS', 'MESSAGES', 'AuthService', '$cookieStore', '$state', '$log', "Session", "$timeout", LoginController]);
})()