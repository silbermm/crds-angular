(function() {

  module.exports = bankInfo;

  bankInfo.$inject = ['$log', '$rootScope', '$timeout'];

  function bankInfo($log, $rootScope, $timeout) {
    var directive = {
      restrict: 'EA',
      replace: true,
      scope: {
        account: '=',
        bankinfoSubmitted: '=',
        changeAccountInfo: '=',
        defaultSource: '=',
        routing: '=',
        showMessage: '=',
        showMessageOnChange: '=',
        showCheckClass: '=',
        declinedPayment: '='
      },
      templateUrl: 'bankInfo/bankInfo.html',
      link: link
    };

    return directive;

    function link(scope, element, attrs) {

      scope.bankAccount = scope;
      scope.accountError = accountError;
      scope.blurAccountError = blurAccountError;
      scope.blurRoutingError = blurRoutingError;
      scope.resetDefaultBankPlaceholderValues = resetDefaultBankPlaceholderValues;
      scope.routingError = routingError;
      scope.toggleCheck = toggleCheck;
      scope.useExistingAccountInfo = useExistingAccountInfo;

      activate();

      ////////////////////////////////
      //// IMPLEMENTATION DETAILS ////
      ////////////////////////////////

      function activate() {
        if (scope.defaultSource !== undefined) {
          if (!scope.defaultSource.bank_account) {
            scope.resetDefaultBankPlaceholderValues();
          } else if (scope.defaultSource.bank_account.last4) {
            scope.bankAccount.account = '';
            scope.bankAccount.routing = '';
            scope.defaultBankPlaceholderValues = {
              routing: scope.defaultSource.bank_account.routing,
              maskedAccount: 'XXXXXXXXXXX' + scope.defaultSource.bank_account.last4
            };

          }
        }
      }

      function accountError() {
        if (scope.useExistingAccountInfo()) {
          return false;
        }

        return (scope.bankinfoSubmitted &&
            scope.bankAccountForm.account.$error.invalidAccount &&
            scope.bankAccountForm.$invalid  ||
            scope.bankAccountForm.account.$error.invalidAccount &&
            scope.bankAccountForm.account.$dirty ||
            scope.showMessageOnChange &&
            scope.bankAccountForm.account.$error.invalidAccount);
      }

      function blurAccountError() {
        if (scope.useExistingAccountInfo()) {
          return false;
        }

        return (scope.bankAccountForm.account.$dirty && scope.bankAccountForm.account.$error.invalidAccount);
      }

      function blurRoutingError() {
        if (scope.useExistingAccountInfo()) {
          return false;
        }

        return (scope.bankAccountForm.routing.$dirty && scope.bankAccountForm.routing.$error.invalidRouting);
      }

      function resetDefaultBankPlaceholderValues() {
        scope.defaultBankPlaceholderValues = {};
        scope.declinedPayment = false;
      }

      function routingError() {
        if (scope.useExistingAccountInfo()) {
          return false;
        }

        return (scope.bankinfoSubmitted &&
            scope.bankAccountForm.routing.$error.invalidRouting &&
            scope.bankAccountForm.$invalid  ||
            scope.bankAccountForm.routing.$error.invalidRouting &&
            scope.bankAccountForm.routing.$dirty ||
            scope.showMessageOnChange &&
            scope.bankAccountForm.routing.$error.invalidRouting);
      }

      function toggleCheck() {
        if (scope.showMessage === 'Where?') {
          scope.showMessage = 'Close';
          scope.showCheckClass = '';
        } else {
          scope.showMessage = 'Where?';
          scope.showCheckClass = 'ng-hide';
        }
      }

      function useExistingAccountInfo() {
        return scope.changeAccountInfo && scope.bankAccountForm.$pristine;
      }

     
    }
  }
})();
