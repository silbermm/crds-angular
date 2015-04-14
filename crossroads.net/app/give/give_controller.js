(function () {
    'use strict';
    module.exports = function GiveCtrl($scope, $log, messages, opportunity) {

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

        _this.alerts = [
            {
                type: 'warning',
                msg: "If it's all the same to you, please use your bank account (credit card companies charge Crossroads a fee for each gift)."
            }
        ]

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
