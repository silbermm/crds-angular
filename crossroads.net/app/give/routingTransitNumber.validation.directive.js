"use strict";
(function () {

  module.exports = function () {

        return {
            restrict: "A",
            require: 'ngModel',
            link: function(scope, element, attrs, ngModel){
                ngModel.$validators.invalidRouting = function (value) {
                   var validRtn = /^[0-9]{9}$/;
                   //I want to perform over-zealous RTN validation.  Holding myself back...maybe later??
                   //var validRtn = /^((0[0-9])|(1[0-2])|(2[1-9])|(3[0-2])|(6[1-9])|(7[0-2])|80)([0-9]{7})$/;
                   var match = validRtn.test(value);
                   return match;
                };
            }

        };
      }
})()
