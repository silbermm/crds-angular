require('./creditCardInfo.html');
(function () {
    angular
    .module('credit-card-info',[])
    .directive('creditCardInfo', ['$log', bankInfo]);

    function bankInfo($log) {
        var directive = {
          link: link,
          replace: true,
          templateUrl: 'give/creditCardInfo.html',
          restrict: 'EA'
      };
      return directive;

      function link(scope, element, attrs) {
        $log.debug("Inside of creditCardInfo directive");
      }
    }
})()
