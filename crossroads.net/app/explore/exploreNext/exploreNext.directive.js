(function () {
  'use strict';

  module.exports = exploreNext;

  exploreNext.$inject = ['$window', '$document'];

  function exploreNext($window, $document) {
    return {
      restrict: "EA",
      templateUrl: function (elem, attr) {
        return 'exploreNext/exploreNext.html';
      },
      scope: {
        anchor: '@',
        text: '@'
      },
      link: link
    };


    function link(scope, element, attrs) {

      function RepositionNav() {
        var windowHeight = $window.innerHeight; //get the height of the window
        var nextElement = element.find("div"); //get the div for the next button
        var navHeight = nextElement[0].offsetHeight; //get the height of the next button
        var navPos = windowHeight - navHeight;
        nextElement.css({"top": navPos+"px"});
      }

      RepositionNav();

      angular.element($window).bind('resize', function() {
        scope.$apply(function() {

          RepositionNav();

      		//$('#meet').css({"height": windowHeight}); //set the new top position of the navigation list
      		//$('.next').css({"top" : navPos}); //set the position of the next link relative to window height


        });
      });

    }
  }
})();
