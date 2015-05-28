require('../creditCardInfo.html');

(function () {
    angular
    .module('crossroads.give')
    .directive('creditCardInfo', ['$log', bankInfo]);

    function bankInfo($log) {
        var directive = {
          restrict: 'EA',
          replace: true,
          scope: {
              nameOnCard: "=",
              ccNumber: "=",
              expDate: "=",
              cvc: "=",
              billingZipCode: "=",
              validForm: "="
            },
          templateUrl: 'give/creditCardInfo.html',
          link: link
      };
      return directive;

      function link(scope, element, attrs) {
        $log.debug("Inside of creditCardInfo directive");

        scope.billingZipCodeError = function() {
          return (vm.bankinfoSubmitted && $scope.creditCardForm.billingZipCode.$invalid ||
            $scope.creditCardForm.billingZipCode.$dirty && $scope.creditCardForm.billingZipCode.$invalid);
        };

        scope.blurBillingZipCodeError = function() {
          return ($scope.creditCardForm.billingZipCode.$dirty && $scope.creditCardForm.billingZipCode.$invalid);
        };
       
        scope.blurRoutingError = function() {
          return ($scope.creditCardForm.routing.$dirty && $scope.creditCardForm.routing.$error.invalidRouting );
        };

        scope.ccCardType = function () {
            if (vm.ccNumber) {
                if (vm.ccNumber.match(visaRegEx))
                  vm.ccNumberClass = "cc-visa";
                else if (vm.ccNumber.match(mastercardRegEx))
                  vm.ccNumberClass = "cc-mastercard";
                else if (vm.ccNumber.match(discoverRegEx))
                  vm.ccNumberClass = "cc-discover";
                else if (vm.ccNumber.match(americanExpressRegEx))
                  vm.ccNumberClass = "cc-american-express";
                else
                  vm.ccNumberClass = "";
            } else
                vm.ccNumberClass = "";
        };

        scope.ccNumberError = function(ccValid) {
            if (ccValid === undefined) {
                vm.setValidCard = false ;
            }

            return (vm.bankinfoSubmitted && $scope.creditCardForm.ccNumber.$pristine || //cannot be blank on submit
                    vm.setValidCard && !vm.bankinfoSubmitted || //can be empty on pageload
                    !ccValid && vm.bankinfoSubmitted ||
                    !ccValid && $scope.creditCardForm.ccNumber.$dirty);  //show error when not valid
         };

         scope.cvvError = function(cvcValid) {
            if (cvcValid === undefined) {
                vm.setValidCvc = false  ;
            }

            return (vm.bankinfoSubmitted && $scope.creditCardForm.cvc.$pristine || //cannot be blank on submit
                    vm.setValidCvc && !vm.bankinfoSubmitted || //can be empty on pageload
                    !cvcValid && vm.bankinfoSubmitted ||
                    !cvcValid && $scope.creditCardForm.cvc.$dirty);  //show error when not valid
        };

        scope.expDateError = function() {
            return (vm.bankinfoSubmitted && $scope.creditCardForm.expDate.$invalid);
        };

         // Emits a growl notification encouraging checking/savings account
        // donations, rather than credit card
        scope.initCreditCardBankSection = function() {
            $rootScope.$emit(
                'notify',
                $rootScope.MESSAGES.creditCardDiscouraged,
                vm.creditCardDiscouragedGrowlDivRef,
                -1 // Indicates that this message should not time out
                );
        };

        scope.nameError = function() {
            return (vm.bankinfoSubmitted && $scope.creditCardForm.nameOnCard.$invalid);
        };

       
      }
    };


})()
