require('./donation-details.html');﻿

(function () {
    angular
    .module("donation-details",[])
    .directive("donationDetails", ['$log','getPrograms', donationDetails]);

    function donationDetails($log , getPrograms) {
        var directive = {
          link: link,
          replace: true,
          templateUrl: 'give/donation-details.html',
          restrict: 'EA'
      };
      return directive;

      function link(scope, element, attrs) {
        scope.programs = getPrograms.fetchPrograms();
      }
    }
})()
