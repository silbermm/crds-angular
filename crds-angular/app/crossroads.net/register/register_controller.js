'use strict';
(function () {
    angular.module('crossroads').controller('RegisterCtrl', ['$scope', '$rootScope', 'AUTH_EVENTS', 'MESSAGES', 'Users', '$log', RegisterController]);

    function RegisterController($scope, $rootScope, AUTH_EVENTS, MESSAGES, Users, $log) {
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
            $log.debug(form.newuser.email);
            $log.debug(form.newuser.firstname);
            $log.debug(form.newuser.password);

            Users.save(form.newuser);
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