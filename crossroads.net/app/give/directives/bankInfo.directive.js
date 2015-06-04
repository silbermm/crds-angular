require('../bankInfo.html');
(function () {
    angular
    .module('crossroads.give')
    .directive('bankInfo', ['$log', '$rootScope', '$timeout', bankInfo]);

    function bankInfo($log, $rootScope, $timeout) {
        var directive = {
          restrict: 'EA',
          replace: true,
          scope: {
              account: "=", 
              bankinfoSubmitted: "=",
              routing: "="             
            },
          templateUrl: 'give/bankInfo.html',
          link: link
      };
      return directive;

      function link(scope, element, attrs) {
        $log.debug("Inside of bankInfo directive");

        scope.bankAccount = scope;

        scope.accountError = function() {
          return (scope.bankinfoSubmitted && scope.bankAccountForm.account.$error.invalidAccount && scope.bankAccountForm.$invalid  ||
            scope.bankAccountForm.account.$error.invalidAccount && scope.bankAccountForm.account.$dirty);
        };

        scope.blurAccountError = function() {
            return (scope.bankAccountForm.account.$dirty && scope.bankAccountForm.account.$error.invalidAccount);
        };

        scope.routingError = function() {
           console.log("in here");
            return (scope.bankinfoSubmitted && scope.bankAccountForm.routing.$error.invalidRouting && scope.bankAccountForm.$invalid  ||
                scope.bankAccountForm.routing.$error.invalidRouting && scope.bankAccountForm.routing.$dirty);
        };

         scope.blurRoutingError = function() {
           console.log("in here ii");
          return (scope.bankAccountForm.routing.$dirty && scope.bankAccountForm.routing.$error.invalidRouting );
        };

        scope.toggleCheck = function() {
            if (scope.showMessage == "Where?") {
                scope.showMessage = "Close";
                scope.showCheckClass = "";
            } else {
                scope.showMessage = "Where?";
                scope.showCheckClass = "ng-hide";
            }
        };



      }
    }
})()
