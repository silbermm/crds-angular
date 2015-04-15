"use strict";
(function () {

  module.exports = function () {

        return {
            restrict: "A",
            require: 'ngModel',
            link: function(scope, element, attrs, ngModel){
                ngModel.$validators.invalidCvv = function (value) {
                  var validCvv = /^[0-9]{3,4}$/;
                  var status = validCvv.test(value);
                  return status;
                };
            }

        };
      }
})()
