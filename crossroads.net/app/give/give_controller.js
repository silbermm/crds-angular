(function () {
    'use strict';

    module.exports = function GiveCtrl($scope, $state, $rootScope, $timeout) {
        var _this = this;
        //Credit Card RegExs
        //var visaRegEx = /^4[0-9]{2}/;
        var visaRegEx = /^4[0-9]{12}(?:[0-9]{3})?$ /;
        var mastercardRegEx = /^5[1-5][0-9]/;
        //var discoverRegEx = /^6(?:011|5[0-9]{2})/;
        var discoverRegEx =/^6(?:011|5[0-9]{2})[0-9]{12}$/;
        //var americanExpressRegEx = /^3[47]/;
        var americanExpressRegEx = /^3[47][0-9]{13}$/;

        _this.view = 'bank';
        _this.bankType = 'checking';
        _this.showMessage = "Where?";
        _this.showCheckClass = "ng-hide";
        _this.email = null;
        _this.emailAlreadyRegisteredGrowlDivRef = 1000;

        // TODO Need to figure out a better option to get to the "initial" state
        $state.go("give.amount");

        _this.alerts = [
            {
                type: 'warning',
                msg: "If it's all the same to you, please use your bank account (credit card companies charge Crossroads a fee for each gift)."
            }
        ]

        _this.onEmailFound = function() {
            $rootScope.$emit(
                'notify'
                , $rootScope.MESSAGES.donorEmailAlreadyRegistered
                , _this.emailAlreadyRegisteredGrowlDivRef
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

        _this.toggleCheck = function() {
            if (_this.showMessage == "Where?") {
                _this.showMessage = "Close";
                _this.showCheckClass = "";
            } else {
                _this.showMessage = "Where?";
                _this.showCheckClass = "ng-hide";
            }
        }

        _this.closeAlert = function (index) {
            _this.alerts.splice(index, 1);
        }

        _this.ccCardType = function () {
            if (_this.ccNumber) {
                if (_this.ccNumber.match(visaRegEx))
                    _this.ccNumberClass = "cc-visa";
                else if (_this.ccNumber.match(mastercardRegEx))
                    _this.ccNumberClass = "cc-mastercard";
                else if (_this.ccNumber.match(discoverRegEx))
                    _this.ccNumberClass = "cc-discover";
                else if (_this.ccNumber.match(americanExpressRegEx))
                    _this.ccNumberClass = "cc-american-express";
                else
                    _this.ccNumberClass = "";
            } else
                _this.ccNumberClass = "";
        }

    };

})();
