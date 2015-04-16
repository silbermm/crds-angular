'use strict';
(function () {
  module.exports = function GiveCtrl($rootScope, $scope, $state, $timeout) {
        var vm = this;
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

        console.log("in the controller");
        // TODO Need to figure out a better option to get to the "initial" state
        $state.go("give.amount");

        vm.alerts = [
            {
                type: 'warning',
                msg: "If it's all the same to you, please use your bank account (credit card companies charge Crossroads a fee for each gift)."
            }
        ]

        _this.onEmailFound = function() {
            $rootScope.$emit(
                'notify'
                , $rootScope.MESSAGES.donorEmailAlreadyRegistered
                , vm.emailAlreadyRegisteredGrowlDivRef
                , -1 // Indicates that this message should not time out
                );
        }

        _this.onEmailNotFound = function() {
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
          console.log(giveForm);
          vm.formValid = true;
           if (!$scope.giveForm.routing.$error.invalidRouting ) {
             console.log("set form valid");
             vm.formValid = true;
             $state.go("give.thank-you");
             }
          if (!vm.formValid) {
            $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
            console.log("emit it here");
          return;
          }
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

        vm.closeAlert = function (index) {
            vm.alerts.splice(index, 1);
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

    };

})();
