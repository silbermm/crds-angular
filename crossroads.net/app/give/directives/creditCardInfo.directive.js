require('../creditCardInfo.html');

(function () {
    angular
    .module('crossroads.give')
    .directive('creditCardInfo', ['$log', bankInfo]);

    //Credit Card RegExs
    var americanExpressRegEx = /^3[47][0-9]{13}$/;
    var discoverRegEx = /^6(?:011|5[0-9]{2})/;
    var mastercardRegEx = /^5[1-5][0-9]/;
    var visaRegEx = /^4[0-9]{12}(?:[0-9]{3})?$/;

    function bankInfo($log) {
        var directive = {
          restrict: 'EA',
          //replace: true,
          scope: {
              nameOnCard: "=",
              ccNumber: "=",
              expDate: "=",
              cvc: "=",
              billingZipCode: "=",
              bankinfoSubmitted: "="
            },
          templateUrl: 'give/creditCardInfo.html',
          link: link
      };
      return directive;

      function link(scope, element, attrs) {
        $log.debug("Inside of creditCardInfo directive");

        scope.creditCardDiscouragedGrowlDivRef = 1001;

        scope.billingZipCodeError = function() {
          console.log("in");
          return (scope.bankinfoSubmitted && scope.creditCardForm.billingZipCode.$invalid ||
            scope.creditCardForm.billingZipCode.$dirty && scope.creditCardForm.billingZipCode.$invalid);
        };

        scope.blurBillingZipCodeError = function() {
          return (scope.creditCardForm.billingZipCode.$dirty && scope.creditCardForm.billingZipCode.$invalid);
        };

        scope.ccCardType = function () {
            if (scope.ccNumber) {
                if (scope.ccNumber.match(visaRegEx))
                  scope.ccNumberClass = "cc-visa";
                else if (scope.ccNumber.match(mastercardRegEx))
                  scope.ccNumberClass = "cc-mastercard";
                else if (scope.ccNumber.match(discoverRegEx))
                  scope.ccNumberClass = "cc-discover";
                else if (scope.ccNumber.match(americanExpressRegEx))
                  scope.ccNumberClass = "cc-american-express";
                else
                  scope.ccNumberClass = "";
            } else
                scope.ccNumberClass = "";
        };

        scope.ccNumberError = function(ccValid) {
            if (ccValid === undefined) {
                scope.setValidCard = false ;
            }

            return (scope.bankinfoSubmitted && scope.creditCardForm.ccNumber.$pristine || //cannot be blank on submit
                    scope.setValidCard && !scope.bankinfoSubmitted || //can be empty on pageload
                    !ccValid && scope.bankinfoSubmitted ||
                    !ccValid && scope.creditCardForm.ccNumber.$dirty);  //show error when not valid
         };

         scope.cvvError = function(cvcValid) {
            if (cvcValid === undefined) {
                scope.setValidCvc = false  ;
            }

            return (scope.bankinfoSubmitted && scope.creditCardForm.cvc.$pristine || //cannot be blank on submit
                    scope.setValidCvc && !scope.bankinfoSubmitted || //can be empty on pageload
                    !cvcValid && scope.bankinfoSubmitted ||
                    !cvcValid && scope.creditCardForm.cvc.$dirty);  //show error when not valid
        };

        scope.expDateError = function() {
            return (scope.bankinfoSubmitted && scope.creditCardForm.expDate.$invalid);
        };

         // Emits a growl notification encouraging checking/savings account
        // donations, rather than credit card
        scope.initCreditCardBankSection = function() {
            $rootScope.$emit(
                'notify',
                $rootScope.MESSAGES.creditCardDiscouraged,
                scope.creditCardDiscouragedGrowlDivRef,
                -1 // Indicates that this message should not time out
                );
        };

        scope.nameError = function() {
            return (scope.bankinfoSubmitted && scope.creditCardForm.nameOnCard.$invalid);
        };


      }
    };


})()
