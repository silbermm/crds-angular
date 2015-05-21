"use strict";
(function () {

  module.exports = function ($timeout) {

 return {
        restrict: 'A',
        link: function ($scope, $element) {
          console.log('Directive used');
          $timeout(function () {
            $element[0].focus();
          });
        }
      };
    }
})()
