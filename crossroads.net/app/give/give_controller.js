(function () {
    'use strict';
    module.exports = function GiveCtrl($scope, $log, messages, opportunity) {
        $scope.view = 'bank';
        $scope.bankType = 'checking';

        $scope.alerts = [
            {
                type: 'warning',
                msg: "If it's all the same to you, please use your bank account (credit card companies charge Crossroads a fee for each gift)."
            }
        ]


        $scope.closeAlert = function (index) {
            $scope.alerts.splice(index, 1);
        };
    };

})();