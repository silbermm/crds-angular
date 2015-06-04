require('../bankCreditCardDetails.html');
(function () {
    angular
    .module('crossroads.give')
    .directive('bankCreditCardDetails', ['$log', bankCreditCardDetails]);

    function bankCreditCardDetails($log) {
        var directive = {
          link: link,
          replace: true,
          templateUrl: 'give/bankCreditCardDetails.html',
          restrict: 'EA'
      };
      return directive;

      function link(scope, element, attrs) {
        $log.debug("Inside of bankCreditCardDetails directive");
      }
    }
})()