'use strict';
(function () {
    angular.module('crossroads').controller('RegisterCtrl', ['$scope', '$rootScope', 'AUTH_EVENTS','AuthService', 'MESSAGES', 'Users', '$log', RegisterController]);
   
    function RegisterController($scope, $rootScope, AUTH_EVENTS, AuthService, MESSAGES, Users, $log) {
        $log.debug("Inside register controller");

        $scope.passwordPrefix = "registration";

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

        $scope.register = function (form) {
            _this.form = form;

            if (form.newuser == null || form.newuser.email == null || form.newuser.password == null || form.newuser.email == "" || form.newuser.password == "" || form.newuser.firstname == null || form.newuser.firstname == '' || form.newuser.lastname == null || form.newuser.lastname =='') {
                $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
                return;
            }
                
            $scope.credentials = {};
            $scope.credentials.username = form.newuser.email;
            $scope.credentials.password = form.newuser.password;

            var user = new Users(form.newuser);
            user.$save().then(function () {
                AuthService.login($scope.credentials).then(function (user) { // TODO Refactor this to a shared location for use here and in login_controller
                    $log.debug("got a 200 from the server ");
                    $log.debug(user);
                    $scope.registerShow = !$scope.registerShow;
                    $rootScope.showLoginButton = false; //TODO use emit or an event here, avoid using rootscope
                    $rootScope.$emit('notify', $rootScope.MESSAGES.successfullRegistration);
                }, function () {
                    $log.debug("Bad password");
                    $scope.pending = false;
                    $scope.loginFailed = true;
                }).then(function () {
                    $scope.processing = false;
                })
            });

        }


        $scope.showRegisterButton = true;
        $scope.registerShow = false;

        $scope.toggleDesktopRegister = function () {
            $scope.registerShow = !$scope.registerShow;
            if ($scope.loginShow)
                $scope.loginShow = !$scope.loginShow;
        }

        $scope.openLogin = function () {
            $scope.registerShow = !$scope.registerShow;
            if (!$scope.loginShow)
                $scope.loginShow = !$scope.loginShow;
        }
    }
})()