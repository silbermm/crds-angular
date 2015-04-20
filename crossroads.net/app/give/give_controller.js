'use strict';
(function () {

  module.exports = function GiveCtrl($rootScope, $scope, $state, $timeout) {

        $scope.$on('$stateChangeStart', function (event, toState, toParams) {
            if(toState.name =="give.thank-you" && $scope.giveForm.giveForm.$invalid){
                $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
                event.preventDefault();
            }

             if(toState.name =="give.account" && $scope.giveForm.giveForm.amount.$error.naturalNumber){
                console.log($scope.giveForm.giveForm.amount.$error);
                $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
                event.preventDefault();
            }
        });

        var vm = this;
        vm.amountSubmitted = false;
        vm.bankinfoSubmitted = false;
        //Credit Card RegExs
         var visaRegEx = /^4[0-9]{12}(?:[0-9]{3})?$ /;
         var mastercardRegEx = /^5[1-5][0-9]/;
         var discoverRegEx = /^6(?:011|5[0-9]{2})/;
         var americanExpressRegEx = /^3[47][0-9]{13}$/;

        vm.view = 'bank';
        vm.bankType = 'checking';
        vm.showMessage = "Where?";
        vm.showCheckClass = "ng-hide";
        vm.email = null;
        vm.emailAlreadyRegisteredGrowlDivRef = 1000;
        vm.creditCardDiscouragedGrowlDivRef = 1001;
        vm.emailPrefix = "give";

        console.log("in the controller");

        // Invoked from the initial "/give" state to get us to the first page
        vm.initDefaultState = function() {
            if($state.is("give")) {
                $state.go("give.amount");
            }
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

        vm.goToAccount = function(){
                vm.amountSubmitted = true;
                $state.go("give.account");
        };
    };

})();
