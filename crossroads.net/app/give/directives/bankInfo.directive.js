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
              changeAccountInfo: "=",
              defaultSource: "=",
              routing: "=" ,
              showMessage: "=" ,
              showMessageOnChange: "=",
              showCheckClass: "=",
              declinedPayment: "="
            },
          templateUrl: 'give/bankInfo.html',
          link: link
      };
      return directive;

      function link(scope, element, attrs) {
        $log.debug("Inside of bankInfo directive");

        scope.bankAccount = scope;

        scope.accountError = function() {
          if(scope.useExistingAccountInfo()) {
            return(false);
          }
          return (scope.bankinfoSubmitted && scope.bankAccountForm.account.$error.invalidAccount && scope.bankAccountForm.$invalid  ||
            scope.bankAccountForm.account.$error.invalidAccount && scope.bankAccountForm.account.$dirty
            || scope.showMessageOnChange && scope.bankAccountForm.account.$error.invalidAccount);
        };

        scope.blurAccountError = function() {
          if(scope.useExistingAccountInfo()) {
            return(false);
          }
          return (scope.bankAccountForm.account.$dirty && scope.bankAccountForm.account.$error.invalidAccount);
        };

        scope.blurRoutingError = function() {
          if(scope.useExistingAccountInfo()) {
            return(false);
          }
          return (scope.bankAccountForm.routing.$dirty && scope.bankAccountForm.routing.$error.invalidRouting );
        };

         scope.resetDefaultBankPlaceholderValues = function() {
          scope.defaultBankPlaceholderValues = {};
          scope.declinedPayment = false;
        };

        scope.routingError = function() {
          if(scope.useExistingAccountInfo()) {
            return(false);
          }
          return (scope.bankinfoSubmitted && scope.bankAccountForm.routing.$error.invalidRouting && scope.bankAccountForm.$invalid  ||
                scope.bankAccountForm.routing.$error.invalidRouting && scope.bankAccountForm.routing.$dirty
                || scope.showMessageOnChange && scope.bankAccountForm.routing.$error.invalidRouting);
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

        scope.useExistingAccountInfo = function() {
          return(scope.changeAccountInfo && scope.bankAccountForm.$pristine);
        };

        if (scope.defaultSource !== undefined){       
          if(!scope.defaultSource.bank_account) {
            scope.resetDefaultBankPlaceholderValues();
          } else if(scope.defaultSource.bank_account.last4) {
            scope.bankAccount.account = "";
            scope.bankAccount.routing = "";
            scope.defaultBankPlaceholderValues = {
              routing: scope.defaultSource.bank_account.routing,
              maskedAccount: "XXXXXXXXXXX" + scope.defaultSource.bank_account.last4
            };

         };
      };

      }
    }
})()
