require('../bankInfo.html');
(function () {
    angular
    .module('crossroads.give')
    .directive('bankInfo', ['$log', bankInfo]);

    function bankInfo($log) {
        var directive = {
          link: link,
          replace: true,
          templateUrl: 'give/bankInfo.html',
          restrict: 'EA'
      };
      return directive;

      function link(scope, element, attrs) {
        $log.debug("Inside of bankInfo directive");
      }
    }
})()
