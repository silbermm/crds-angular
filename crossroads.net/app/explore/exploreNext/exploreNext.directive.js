(function () {
  'use strict';

  module.exports = exploreNext;

  exploreNext.$inject = ['$window'];

  function exploreNext($window) {
    return {
      restrict: "EA",
      templateUrl: function (elem, attr) {
        return 'exploreNext/exploreNext.html';
      },
      scope: {
        anchor: '@',
      },
      link: link
    };

    function link(scope, element, attrs) {
      angular.element($window).bind('resize', function() {
        scope.$apply(function() {
          console.log($window.innerHeight);
          console.log($window.innerWidth);
        });
      });

    }
  }
})();
