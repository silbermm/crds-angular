require('../creditCardInfo.html');

(function () {
    angular
    .module('crossroads.give')
    .directive('creditCardInfo', ['$log', '$rootScope', '$timeout', creditCardInfo]);

    //Credit Card RegExs
    var americanExpressRegEx = /^3[47][0-9]{13}$/;
    var discoverRegEx = /^6(?:011|5[0-9]{2})/;
    var mastercardRegEx = /^5[1-5][0-9]/;
    var visaRegEx = /^4[0-9]{12}(?:[0-9]{3})?$/;

    function creditCardInfo($log, $rootScope, $timeout) {
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
              changeAccountInfo: "=",
              setValidCard: "=",
              setValidCvc: "=",
              ccNumberClass: "=",
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
            } else if(scope.defaultCardPlaceholderValues.brand) {
              if (scope.defaultCardPlaceholderValues.brand == "Visa") {
                scope.ccNumberClass = "cc-visa";
              }
              else if (scope.defaultCardPlaceholderValues.brand == "MasterCard") {
                scope.ccNumberClass = "cc-mastercard";
              }
              else if (scope.defaultCardPlaceholderValues.brand == "Discover") {
                scope.ccNumberClass = "cc-discover";
              }
              else if (scope.defaultCardPlaceholderValues.brand == "American Express") {
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
            if (ccValid === true) {
                scope.setValidCard = true ;
            }

            if(scope.useExistingAccountInfo()) {
              return(false);
            }

            return (scope.bankinfoSubmitted && scope.creditCardForm.ccNumber.$pristine && ccValid || //cannot be blank on submit
                   !ccValid && scope.creditCardForm.ccNumber.$dirty);  //show error when not valid
         };

         scope.cvvError = function(cvcValid) {
            if (cvcValid === undefined) {
                scope.setValidCvc = false  ;
            }
            if (cvcValid === true) {
                scope.setValidCvc = true ;
            }

            if(scope.useExistingAccountInfo()) {
              return(false);
            }

            return (scope.bankinfoSubmitted && scope.creditCardForm.cvc.$pristine || //cannot be blank on submit
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

        // This function swaps the expDate field with the current value placeholder
        // for the expDate field with the "MM/YY" placeholder.  This works around
        // an issue with using ui-mask and a placeholder value, otherwise we'd
        // simply use a dynamic placeholder.
        scope.swapCreditCardExpDateFields = function() {
          if(scope.changeAccountInfo) {
            scope.creditCardForm.$setDirty();
            $timeout(function() {
              // The third field is the expDate
              var e = element.find('input')[2];
              e.focus();
            });
          }
        }

        scope.resetDefaultCardPlaceholderValues = function() {
          scope.defaultCardPlaceholderValues = {
            expDate: "MM/YY",
          };
        }

        if(!scope.defaultSource.credit_card) {
          scope.resetDefaultCardPlaceholderValues();
        } else if(scope.defaultSource.credit_card.last4) {
          scope.creditCard.nameOnCard = "";
          scope.creditCard.ccNumber = "";
          scope.creditCard.expDate = "";
          scope.creditCard.cvc = "";
          scope.creditCard.billingZipCode = "";
          scope.defaultCardPlaceholderValues = {
            billingZipCode: scope.defaultSource.credit_card.address_zip,
            brand: scope.defaultSource.credit_card.brand,
            cvc: "XXX",
            expDate: scope.defaultSource.credit_card.exp_date.replace(/^(..)(..).*$/, "$1/$2"),
            nameOnCard: scope.defaultSource.credit_card.name,
            maskedCard: "XXXXXXXXXXX" + scope.defaultSource.credit_card.last4
          };

          scope.ccCardType();
        }
      }
    };


})()
