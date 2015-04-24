'use strict';
(function () {

  var getCookie = require('../utilities/cookies'); 
  module.exports = function GiveCtrl($rootScope, $scope, $state, $timeout, $httpProvider, Session, Profile) {

        $scope.$on('$stateChangeStart', function (event, toState, toParams) {
           if ($rootScope.email) {   
               vm.email = $rootScope.email;
               //what if email is not found for some reason??
             }
                  
            if (toState.name =="give.thank-you" && $scope.giveForm.giveForm.$invalid){
                $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
                event.preventDefault();
            }
        });

        var vm = this;
        
        //Credit Card RegExs
        var americanExpressRegEx = /^3[47][0-9]{13}$/;
        var discoverRegEx = /^6(?:011|5[0-9]{2})/;
        var mastercardRegEx = /^5[1-5][0-9]/;
        var visaRegEx = /^4[0-9]{12}(?:[0-9]{3})?$/;
        
        vm.setValidCvc = '';
        vm.setValidCard = '';
        vm.amountSubmitted = false;
        vm.bankinfoSubmitted = false;
        vm.bankType = 'checking';
        vm.creditCardDiscouragedGrowlDivRef = 1001;
        vm.email = null;
        vm.emailAlreadyRegisteredGrowlDivRef = 1000;
        vm.emailPrefix = "give";
        vm.showMessage = "Where?";
        vm.showCheckClass = "ng-hide";
        vm.view = 'bank';

        vm.accountError = function() {
            return (vm.bankinfoSubmitted && $scope.giveForm.giveForm.account.$error.invalidAccount ||
                $scope.giveForm.giveForm.account.$dirty && $scope.giveForm.giveForm.account.$error.invalidAccount
                && $scope.giveForm.giveForm.account.$viewValue !== '')
        };

        vm.amountError = function() {
            return (vm.amountSubmitted && $scope.giveForm.giveForm.$invalid)
        };

        vm.billingZipCodeError = function() {
            return (vm.bankinfoSubmitted && $scope.giveForm.giveForm.billingZipCode.$invalid)  
        };

        vm.blurAccountError = function() {
            return ($scope.giveForm.giveForm.account.$dirty && $scope.giveForm.giveForm.account.$error.invalidAccount
                && $scope.giveForm.giveForm.account.$viewValue !=='')
        };

        vm.blurBillingZipCodeError = function() {
            return ($scope.giveForm.giveForm.billingZipCode.$dirty && $scope.giveForm.giveForm.billingZipCode.$invalid)  
        };
        
        vm.blurRoutingError = function() {
            return ($scope.giveForm.giveForm.routing.$dirty && $scope.giveForm.giveForm.routing.$error.invalidRouting 
                && $scope.giveForm.giveForm.routing.$viewValue !=='')
        };  

        vm.ccNumberError = function(ccValid) {
            if (ccValid == undefined) {
                vm.setValidCard = true  ;
            } else {
                vm.setValidCard = ccValid;
            }
           // !give.ccValid && giveForm.ccNumber.$dirty ||giveForm.ccNumber.$error.required && give.bankinfoSubmitted
            return (vm.bankinfoSubmitted && $scope.giveForm.giveForm.ccNumber.$pristine ||
                vm.setValidCard && $scope.giveForm.giveForm.ccNumber.$dirty && !vm.bankinfoSubmitted ||
                !vm.setValidCard || !vm.setValidCard && $scope.giveForm.giveForm.ccNumber.$isEmpty)  
        };
       
         vm.cvvError = function(cvcValid) {
            if (cvcValid == undefined) {
                vm.setValidCvc = true  ;
            } else {
                vm.setValidCvc = cvcValid;
            }
            
            return (vm.bankinfoSubmitted && $scope.giveForm.giveForm.cvc.$pristine ||
                !vm.setValidCvc || !vm.setValidCvc && $scope.giveForm.giveForm.cvc.$isEmpty)  
        };

        vm.expDateError = function() {
            return (vm.bankinfoSubmitted && $scope.giveForm.giveForm.expDate.$invalid)             
        };
       
        // Invoked from the initial "/give" state to get us to the first page
        vm.initDefaultState = function() {
            $scope.$on('$viewContentLoaded', function() {
                if($state.is("give")) {
                    $state.go("give.amount");
                }
            });
        }

        // Emits a growl notification encouraging checking/savings account
        // donations, rather than credit card
        vm.initCreditCardBankSection = function() {
            $rootScope.$emit(
                'notify'
                , $rootScope.MESSAGES.creditCardDiscouraged
                , vm.creditCardDiscouragedGrowlDivRef
                , -1 // Indicates that this message should not time out
                );
        }

        vm.nameError = function() {
            return (vm.bankinfoSubmitted && $scope.giveForm.giveForm.nameOnCard.$invalid)             
        };

        // Callback from email-field on guest giver page.  Emits a growl
        // notification indicating that the email entered may already be a
        // registered user.
        vm.onEmailFound = function() {
            $rootScope.$emit(
                'notify'
                , $rootScope.MESSAGES.donorEmailAlreadyRegistered
                , vm.emailAlreadyRegisteredGrowlDivRef
                , -1 // Indicates that this message should not time out
                );
        }

        // Callback from email-field on guest giver page.  This closes any
        // growl notification left over from the onEmailFound callback.
        vm.onEmailNotFound = function() {
            // There isn't a way to close growl messages in code, outside of the growl
            // directive itself.  To work around this, we'll simply trigger the "click"
            // event on the close button, which has a close handler function.
            var closeButton = document.querySelector("#existingEmail .close");
            if(closeButton !== undefined) {
                $timeout(function() {
                    angular.element(closeButton).triggerHandler("click");
                }, 0);
            }
        }

        vm.routingError = function() {
            return (vm.bankinfoSubmitted  && $scope.giveForm.giveForm.routing.$error.invalidRouting ||
                $scope.giveForm.giveForm.routing.$dirty && $scope.giveForm.giveForm.routing.$error.invalidRouting 
                && $scope.giveForm.giveForm.routing.$viewValue!== '')
         };

        vm.submitBankInfo = function() {
            vm.bankinfoSubmitted = true;
            $state.go("give.thank-you");
        };

        vm.toggleCheck = function() {
            if (vm.showMessage == "Where?") {
                vm.showMessage = "Close";
                vm.showCheckClass = "";
            } else {
                vm.showMessage = "Where?";
                vm.showCheckClass = "ng-hide";
            }
        }

        vm.ccCardType = function () {
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
        }

        vm.goToAccount = function() {
            vm.amountSubmitted = true;
            if($scope.giveForm.giveForm.giveForm.$valid) {
                if ($rootScope.username == undefined) {
                    Session.addRedirectRoute("give.account", "");
                    $state.go("give.login"); 
                } else {
                    $state.go("give.account");
                }
            } else {
               $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
            }
        }
        
        vm.goToLogin = function () {
          Session.addRedirectRoute("give.account", "");
          $state.go("give.login");
        }
    };

})();
