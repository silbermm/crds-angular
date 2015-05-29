require('../bankInfo.html');
(function () {
    angular
    .module('crossroads.give')
    .directive('bankInfo', ['$log', bankInfo]);

    function bankInfo($log) {
        var directive = {
          restrict: 'EA',
          replace: true,
          scope: {
              routing: "=",
              account: "="              
            },
          templateUrl: 'give/bankInfo.html',
          linl: link
      };
      return directive;

      function link(scope, element, attrs) {
        $log.debug("Inside of bankInfo directive");

        scope.accountError = function() {
          return (vm.bankinfoSubmitted && $scope.bankAccountForm.account.$error.invalidAccount && $scope.bankAccountForm.$invalid  ||
            $scope.bankAccountForm.account.$error.invalidAccount && $scope.bankAccountForm.account.$dirty);
        };

        scope.blurAccountError = function() {
          return ($scope.bankAccountForm.account.$dirty && $scope.bankAccountForm.account.$error.invalidAccount);
        };

        scope.routingError = function() {
            return (vm.bankinfoSubmitted && $scope.bankAccountForm.routing.$error.invalidRouting && $scope.bankAccountForm.$invalid  ||
                $scope.bankAccountForm.routing.$error.invalidRouting && $scope.bankAccountForm.routing.$dirty);
        };

         scope.blurRoutingError = function() {
          return ($scope.creditCardForm.routing.$dirty && $scope.creditCardForm.routing.$error.invalidRouting );
        };

        scope.toggleCheck = function() {
            if (vm.showMessage == "Where?") {
                vm.showMessage = "Close";
                vm.showCheckClass = "";
            } else {
                vm.showMessage = "Where?";
                vm.showCheckClass = "ng-hide";
            }
        };



      }
    }
})()
