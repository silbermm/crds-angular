require('../donation-details.html');﻿

(function () {
    angular
    .module("crossroads.give")
    .directive("donationDetails", ['$log','getPrograms', donationDetails]);

    function donationDetails($log , getPrograms) {
        var directive = {
          restrict: 'EA',
          replace: true,
          scope: {
                progtype: "=",
                give: "=",
                amount: "=",
                program: "=",
                amountSubmitted: "="
            },
          templateUrl: 'give/donation-details.html',
          link: link
      };
      return directive;

      function link(scope, element, attrs) {
        getPrograms.Programs.get({programType: scope.progtype}).$promise.then(function(response){
        scope.programs = response;
        scope.program = scope.programs[0];
        });

        scope.amountError = function() {
            return (scope.mountSubmitted && scope.donationDetailsForm.$invalid && scope.donationDetailsForm.$error.naturalNumber || scope.donationDetailsForm.$dirty && scope.donationDetailsForm.$invalid)
        };
      }
    }
})()
