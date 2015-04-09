require('./donation-details.html');﻿

(function () {
    angular
    .module("donation-details",[])
    .directive("donationDetails", ['$log', donationDetails]);

    function donationDetails($log) {
        var directive = {
          link: link,
          replace: true,
          templateUrl: 'give/donation-details.html',
          restrict: 'EA'
      };
      return directive;

      function link(scope, element, attrs) {

        $log.debug("Inside of donationDetails directive");
      }
    }
})()
