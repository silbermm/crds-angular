"use strict";
(function () {

  module.exports = function () {

        return {
            restrict: "A",
            require: 'ngModel',
            link: function(scope, element, attrs, ngModel){
                ngModel.$validators.invalidCredit = function (value) {
                   return true;
                };
            }

        };
      }
})()
