"use strict";
(function () {

  module.exports = function () {

        return {
            restrict: "A",
            require: 'ngModel',
            link: function(scope, element, attrs, ngModel){
                ngModel.$validators.invalidAccount = function (value) {
                   var validAccount = /^[1-30]\d*$/;
                   var match = validAccount.test(value);
                   return match;
                };
            }

        };
      }
})()
