(function () {
    'use strict';
    module.exports = function GiveCtrl($scope, $log, messages, opportunity) {

        var _this = this;
        //Credit Card RegExs
        var visaRegEx = /^4[0-9]{2}/;
        var mastercardRegEx = /^5[1-5][0-9]/;
        var discoverRegEx = /^6(?:011|5[0-9]{2})/;
        var americanExpressRegEx = /^3[47]/;

        _this.view = 'bank';
        _this.bankType = 'checking';

        _this.alerts = [
            {
                type: 'warning',
                msg: "If it's all the same to you, please use your bank account (credit card companies charge Crossroads a fee for each gift)."
            }
        ]


        _this.closeAlert = function (index) {
            _this.alerts.splice(index, 1);
        }

        _this.ccCardType = function () {
            if (_this.ccNumber.length > 3) {
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