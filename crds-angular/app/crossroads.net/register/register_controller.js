'use strict';
(function () {
    angular.module('crossroads').controller('RegisterCtrl', ['$scope', '$rootScope', 'AUTH_EVENTS','AuthService', 'MESSAGES', 'Users', '$log', RegisterController]);

    function RegisterController($scope, $rootScope, AUTH_EVENTS, AuthService, MESSAGES, Users, $log) {
        $log.debug("Inside register controller");

        $scope.pwprocess = function(){
            if ($scope.pwprocessing =="SHOW") {
                $scope.pwprocessing = "HIDE";
                $scope.inputType = 'text';
            }
            else {
                $scope.pwprocessing = "SHOW";
                $scope.inputType = 'password';
            }
        }

        var _this = this;
        //var users = User;

        $scope.register = function (form) {
            _this.form = form;
            $scope.credentials.username = form.newuser.email;
            $scope.credentials.password = form.newuser.password;
            Users.save(form.newuser).then(AuthService.login($scope.credentials).then(function (user) {
                $log.debug("got a 200 from the server ");
                $log.debug(user);
                $scope.processing = false;
                $scope.loginShow = false;
                $scope.showLoginButton = false;
            }));

        }


        $scope.showRegisterButton = true;
        $scope.registerShow = false;

        $scope.toggleDesktopRegister = function () {
            $scope.registerShow = !$scope.registerShow;
            if ($scope.loginShow)
                $scope.loginShow = !$scope.loginShow;
        }
    }
})()