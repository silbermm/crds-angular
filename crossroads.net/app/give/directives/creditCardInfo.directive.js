require('../creditCardInfo.html');

(function () {
    angular
    .module('crossroads.give')
    .directive('creditCardInfo', ['$log', '$rootScope', bankInfo]);

    //Credit Card RegExs
    var americanExpressRegEx = /^3[47][0-9]{13}$/;
    var discoverRegEx = /^6(?:011|5[0-9]{2})/;
    var mastercardRegEx = /^5[1-5][0-9]/;
    var visaRegEx = /^4[0-9]{12}(?:[0-9]{3})?$/;

    function bankInfo($log, $rootScope, growl) {
        var directive = {
          restrict: 'EA',
          //replace: true,
          scope: {
              nameOnCard: "=",
              ccNumber: "=",
              expDate: "=",
              cvc: "=",
              billingZipCode: "=",
              bankinfoSubmitted: "=",
              defaultSource: "=",
              changeAccountInfo: "@",
            },
          templateUrl: 'give/creditCardInfo.html',
          link: link
      };
      return directive;

      function link(scope, element, attrs) {
        $log.debug("Inside of creditCardInfo directive");

        scope.creditCard = scope;

        // Emits a growl notification encouraging checking/savings account
        // donations, rather than credit card
        $rootScope.$emit(
            'notify',
            $rootScope.MESSAGES.creditCardDiscouraged,
            1001,
            -1 // Indicates that this message should not time out
            );

        scope.billingZipCodeError = function() {
          if(scope.useExistingAccountInfo()) {
            return(false);
          }

          return (scope.bankinfoSubmitted && scope.creditCardForm.billingZipCode.$invalid ||
            scope.creditCardForm.billingZipCode.$dirty && scope.creditCardForm.billingZipCode.$invalid);
        };

        scope.blurBillingZipCodeError = function() {
          if(scope.useExistingAccountInfo()) {
            return(false);
          }

          return (scope.creditCardForm.billingZipCode.$dirty && scope.creditCardForm.billingZipCode.$invalid);
        };

        scope.ccCardType = function () {
            var ccNumber = scope.creditCardForm.ccNumber;
            if (ccNumber && ccNumber.$modelValue) {
                ccNumber = ccNumber.$modelValue;
              if (ccNumber.match(visaRegEx))
                scope.ccNumberClass = "cc-visa";
              else if (ccNumber.match(mastercardRegEx))
                scope.ccNumberClass = "cc-mastercard";
              else if (ccNumber.match(discoverRegEx))
                scope.ccNumberClass = "cc-discover";
              else if (ccNumber.match(americanExpressRegEx))
                scope.ccNumberClass = "cc-american-express";
              else
                scope.ccNumberClass = "";
            } else if(scope.defaultCardInfo.brand) {
              if (scope.defaultCardInfo.brand == "Visa") {
                scope.ccNumberClass = "cc-visa";
              }
              else if (scope.defaultCardInfo.brand == "MasterCard") {
                scope.ccNumberClass = "cc-mastercard";
              }
              else if (scope.defaultCardInfo.brand == "Discover") {
                scope.ccNumberClass = "cc-discover";
              }
              else if (scope.defaultCardInfo.brand == "American Express") {
                scope.ccNumberClass = "cc-american-express";
              }
              else {
                scope.ccNumberClass = "";
              }
            } else {
              scope.ccNumberClass = "";
            }
        };

        scope.useExistingAccountInfo = function() {
          return(scope.changeAccountInfo && scope.creditCardForm.$pristine);
        }

        scope.ccNumberError = function(ccValid) {
            if (ccValid === undefined) {
                scope.setValidCard = false ;
            }

            if(scope.useExistingAccountInfo()) {
              return(false);
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

            if(scope.useExistingAccountInfo()) {
              return(false);
            }

            return (scope.bankinfoSubmitted && scope.creditCardForm.cvc.$pristine || //cannot be blank on submit
                    scope.setValidCvc && !scope.bankinfoSubmitted || //can be empty on pageload
                    !cvcValid && scope.bankinfoSubmitted ||
                    !cvcValid && scope.creditCardForm.cvc.$dirty);  //show error when not valid
        };

        scope.expDateError = function() {
          if(scope.useExistingAccountInfo()) {
            return(false);
          }

            return (scope.bankinfoSubmitted && scope.creditCardForm.expDate.$invalid);
        };

        scope.nameError = function() {
          if(scope.useExistingAccountInfo()) {
            return(false);
          }

            return (scope.bankinfoSubmitted && scope.creditCardForm.nameOnCard.$invalid);
        };

        if(!scope.defaultSource) {
          scope.defaultCardInfo = {};
        } else if(scope.defaultSource.last4) {
          scope.creditCard.nameOnCard = "";
          scope.creditCard.ccNumber = "";
          scope.creditCard.expDate = "";
          scope.creditCard.cvc = "";
          scope.creditCard.billingZipCode = "";
          scope.defaultCardInfo = {
            billingZipCode: scope.defaultSource.address_zip,
            brand: scope.defaultSource.brand,
            cvc: "XXX",
            // TODO Hard-coding expDate - should get this from the scope.defaultSource
            expDate: "12/19",
            nameOnCard: scope.defaultSource.name,
            maskedCard: "XXXXXXXXXXX" + scope.defaultSource.last4
          };

          scope.ccCardType();
        }
      }
    };


})()
