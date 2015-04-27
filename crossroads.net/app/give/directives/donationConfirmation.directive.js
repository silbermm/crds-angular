require('./donationConfirmation.html');
(function () {
    angular
    .module('donation-confirmation',[])
    .directive('donationConfirmation', ['$log', donationConfirmation]);

    function donationConfirmation($log) {
        var directive = {
          link: link,
          replace: true,
          templateUrl: 'give/donationConfirmation.html',
          restrict: 'EA'
      };
      return directive;

      function link(scope, element, attrs) {
        $log.debug("Inside of cnfirm directive");
      }
    }
})()
