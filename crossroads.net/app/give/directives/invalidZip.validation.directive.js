"use strict";
(function () {

  module.exports = function () {

        return {
            restrict: "A",
            require: 'ngModel',
            link: function(scope, element, attrs, ngModel){
                ngModel.$validators.invalidZip = function (value) {
                  var validZip = /^\d{5}(?:[-\s]\d{4})?$/;
                  var status = validZip.test(value);
                  return status;
                };
            }

        };
      }
})()
