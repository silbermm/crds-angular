    angular.module('password_field',[]).controller('PasswordFieldCtrl', ['$scope','$log', PasswordFieldController]);

    function PasswordFieldController($scope,$log) {
        $log.debug("Inside PasswordFieldController");

        $scope.AAAinputType = 'password';
        $scope.AAApwprocessing = "SHOW";

        $scope.AAApwprocess = function () {
            $log.debug("pwprocess function launched");
            if ($scope.pwprocessing == "SHOW") {
                $scope.pwprocessing = "HIDE";
                $scope.inputType = 'text';
            }
            else {
                $scope.pwprocessing = "SHOW";
                $scope.inputType = 'password';
            }
            $log.debug($scope.pwprocessing);
        }

    };
